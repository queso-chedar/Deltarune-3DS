using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ZoomLayers : MonoBehaviour
{
    [Header("Configuración de las capas")]
    public GameObject prefabCapa;      // Prefab con RawImage o SpriteRenderer.
    public int numeroCapas = 20;       // Instancias visibles reales (usa 4–6 si puedes).
    public float escalaMinima = 2f;
    public float escalaMaxima = 5f;
    public float velocidadEscala = 0.1f; // unidades de escala / segundo

    [Header("Visibilidad / Profundidad")]
    [Tooltip("Potencia para atenuar por profundidad (2 = cuadrático). Front = menos alpha.")]
    public float depthAlphaPow = 2f;
    [Tooltip("Clamp de alpha para las capas muy frontales (depth < este umbral).")]
    [Range(0f, 1f)] public float frontClampDepth = 0.25f;
    [Tooltip("Máximo alpha permitido en la zona frontal.")]
    [Range(0f, 1f)] public float frontMaxAlpha = 0.15f;
    [Tooltip("Alpha mínimo global para evitar apagón total (0 = permitir 0).")]
    [Range(0f, 0.2f)] public float minAlpha = 0.02f;

    [Header("Optimización")]
    [Tooltip("Solo escribir alpha cada N frames (1 = cada frame).")]
    public int alphaUpdateEveryNFrames = 3;
    [Tooltip("Evitar escrituras si el cambio es menor a eps.")]
    public float eps = 0.001f;

    // --- Interno ---
    class Capa {
        public Transform transform;
        public RawImage rawImage;
        public SpriteRenderer sprite;
        public Color colorBase;
        public float escalaActual;
        public float lastAlpha = -1f;
        public float lastScale = -1f;
    }

    readonly List<Capa> capas = new List<Capa>();
    float rango;
    int frameCounter = 0;
    int runningSortingOrder = 0; // para SpriteRenderer, si usas sprites

    void Start()
    {
        if (prefabCapa == null) {
            Debug.LogError("Debe asignar un Prefab de capa con RawImage o SpriteRenderer.");
            enabled = false;
            return;
        }

        rango = Mathf.Max(0.0001f, escalaMaxima - escalaMinima);
        CrearCapasInicialesEscalonadas();
    }

    void CrearCapasInicialesEscalonadas()
    {
        capas.Clear();

        for (int i = 0; i < numeroCapas; i++)
        {
            GameObject go = Instantiate(prefabCapa, transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;

            var capa = new Capa();
            capa.transform = go.transform;
            capa.rawImage = go.GetComponent<RawImage>();
            capa.sprite = go.GetComponent<SpriteRenderer>();

            if (capa.rawImage == null && capa.sprite == null)
            {
                Debug.LogError("El prefab debe tener RawImage o SpriteRenderer.");
                enabled = false;
                return;
            }

            if (capa.rawImage) {
                capa.colorBase = capa.rawImage.color;
                capa.rawImage.raycastTarget = false;
            } else {
                capa.colorBase = capa.sprite.color;
            }

            // Distribuye uniformemente en el ciclo (escalonado)
            float t = (float)i / numeroCapas;               // 0..1
            float escala = Mathf.Lerp(escalaMinima, escalaMaxima, t);
            capa.escalaActual = escala;
            capa.transform.localScale = Vector3.one * escala;

            // Sibling: el más pequeño (frontal) debe ir al frente (último índice)
            // Como t crece con escala, invertimos el índice:
            capa.transform.SetSiblingIndex(numeroCapas - 1 - i);

            // SpriteRenderer: orden creciente para poner al frente los pequeños
            if (capa.sprite != null)
                capa.sprite.sortingOrder = runningSortingOrder++;

            capas.Add(capa);

            // Alpha inicial coherente
            float alpha = CalcularAlpha(escala);
            AplicarAlpha(capa, alpha, force:true);
        }
    }

    void Update()
    {
        if (capas.Count == 0) return;

        frameCounter++;
        bool writeAlphaThisFrame = (alphaUpdateEveryNFrames <= 1) || (frameCounter % alphaUpdateEveryNFrames == 0);

        float dt = Time.deltaTime;
        for (int i = 0; i < capas.Count; i++)
        {
            var c = capas[i];
            float nuevaEscala = c.escalaActual + velocidadEscala * dt;

            bool wrapped = false;
            if (nuevaEscala > escalaMaxima) {
                // recicla suavemente (evita salto visual)
                nuevaEscala = escalaMinima + (nuevaEscala - escalaMaxima);
                wrapped = true;
            }

            // Escribe scale solo si cambia perceptiblemente
            if (Mathf.Abs(nuevaEscala - c.lastScale) > eps) {
                c.transform.localScale = Vector3.one * nuevaEscala;
                c.lastScale = nuevaEscala;
            }

            c.escalaActual = nuevaEscala;

            // Si recicló, llévalo al frente sin hacer Sort completo
            if (wrapped)
            {
                // UI: poner al frente
                c.transform.SetAsLastSibling();

                // Sprites: adelantar orden
                if (c.sprite != null)
                    c.sprite.sortingOrder = runningSortingOrder++;
            }

            // Alpha
            if (writeAlphaThisFrame)
            {
                float alpha = CalcularAlpha(nuevaEscala);
                // Evita escribir si casi no cambia
                if (Mathf.Abs(alpha - c.lastAlpha) > eps)
                    AplicarAlpha(c, alpha, force:false);
            }
        }
    }

    // --- Alpha shaping ---
    // tDepth: 0 (escalaMinima, frontal) -> 1 (escalaMaxima, fondo)
    float CalcularAlpha(float escala)
    {
        float tDepth = Mathf.InverseLerp(escalaMinima, escalaMaxima, escala); // 0..1

        // Envolvente triangular (pico en medio del ciclo)
        // tri = 0 en 0/1, 1 en 0.5
        float tri = 1f - Mathf.Abs(2f * tDepth - 1f);

        // Atenúa por profundidad: frontal (tDepth~0) casi transparente, fondo más visible
        float depthFactor;
        if (Mathf.Approximately(depthAlphaPow, 2f)) {
            depthFactor = tDepth * tDepth;                 // pow 2 rápido
        } else if (Mathf.Approximately(depthAlphaPow, 3f)) {
            depthFactor = tDepth * tDepth * tDepth;        // pow 3 rápido
        } else {
            depthFactor = Mathf.Pow(tDepth, Mathf.Max(0.01f, depthAlphaPow));
        }

        float alpha = tri * depthFactor;

        // Limitar opacidad en la zona frontal (por si acaso)
        if (tDepth < frontClampDepth)
            alpha = Mathf.Min(alpha, frontMaxAlpha);

        // Piso para evitar apagón total
        if (minAlpha > 0f)
            alpha = Mathf.Max(alpha, minAlpha);

        return Mathf.Clamp01(alpha);
    }

    void AplicarAlpha(Capa c, float a, bool force)
    {
        if (c.rawImage != null)
        {
            var col = c.colorBase; col.a = a;
            c.rawImage.color = col;
        }
        else if (c.sprite != null)
        {
            var col = c.colorBase; col.a = a;
            c.sprite.color = col;
        }
        c.lastAlpha = a;
    }
}
