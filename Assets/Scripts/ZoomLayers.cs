using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ZoomLayers : MonoBehaviour
{
    [Header("Configuración de las capas")]
    public GameObject prefabCapa;    // Prefab con RawImage o SpriteRenderer con la textura.
    public int numeroCapas = 50;     // Cantidad de capas (puede ser hasta 300).
    public float escalaMinima = 1f;  // Escala inicial de cada capa.
    public float escalaMaxima = 10f; // Escala máxima antes de reciclar.
    public float velocidadEscala = 1f; // Velocidad de aumento de escala (unidades/seg).

    // Clase interna para guardar datos de cada capa
    class Capa {
        public Transform transform;
        public RawImage rawImage;
        public SpriteRenderer sprite;
        public Color colorBase;
        public float escalaActual;
    }

    List<Capa> capas = new List<Capa>();

    void Start()
    {
        if (prefabCapa == null) {
            Debug.LogError("Debe asignar un Prefab de capa con RawImage o SpriteRenderer.");
            return;
        }
        float rango = escalaMaxima - escalaMinima;
        // Crear instancias de las capas
        for (int i = 0; i < numeroCapas; i++)
        {
            // Calcular escala inicial escalonada (de mayor a menor)
            float escala = escalaMinima + rango * (numeroCapas - 1 - i) / (numeroCapas - 1);
            GameObject go = Instantiate(prefabCapa, transform);
            go.transform.localScale = Vector3.one * escala;
            go.transform.localPosition = Vector3.zero;
            Capa capa = new Capa();
            capa.transform = go.transform;
            capa.escalaActual = escala;
            // Detectar tipo de componente (UI o Sprite)
            capa.rawImage = go.GetComponent<RawImage>();
            capa.sprite = go.GetComponent<SpriteRenderer>();
            if (capa.rawImage != null) {
                capa.colorBase = capa.rawImage.color;
            } else if (capa.sprite != null) {
                capa.colorBase = capa.sprite.color;
            } else {
                Debug.LogError("El prefab debe tener RawImage o SpriteRenderer.");
            }
            capas.Add(capa);
        }
        // Inicialmente ordenar capas (mayor escala atrás, menor adelante)
        OrdenarCapasPorEscala();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        float rango = escalaMaxima - escalaMinima;
        foreach (var capa in capas)
        {
            // Aumentar escala y reciclar si excede el máximo
            float nuevaEscala = capa.escalaActual + velocidadEscala * dt;
            if (nuevaEscala > escalaMaxima) {
                nuevaEscala = escalaMinima + (nuevaEscala - escalaMaxima);
            }
            capa.escalaActual = nuevaEscala;
            capa.transform.localScale = Vector3.one * nuevaEscala;

            // Calcular transparencia (alpha) con fade-in y fade-out
            float progreso = (nuevaEscala - escalaMinima) / rango; // de 0 a 1
            float alpha;
            float umbralFadeIn = 0.1f;
            if (progreso <= umbralFadeIn) {
                alpha = progreso / umbralFadeIn; // fade-in en el 10% inicial
            } else {
                alpha = (1f - progreso) / (1f - umbralFadeIn); // fade-out restante
            }
            alpha = Mathf.Clamp01(alpha);

            // Aplicar color con el alpha calculado
            if (capa.rawImage != null) {
                Color c = capa.colorBase;
                c.a = alpha;
                capa.rawImage.color = c;
            } else if (capa.sprite != null) {
                Color c = capa.colorBase;
                c.a = alpha;
                capa.sprite.color = c;
            }
        }
        // Reordenar capas por escala en cada frame para mantener el orden de dibujo
        OrdenarCapasPorEscala();
    }

    // Ordena las capas de mayor escala (detrás) a menor escala (al frente).
    void OrdenarCapasPorEscala()
    {
        capas.Sort((a, b) => b.escalaActual.CompareTo(a.escalaActual));
        for (int i = 0; i < capas.Count; i++)
        {
            // Para UI RawImage: ajustar índice de hermano (índice mayor = al frente)
            if (capas[i].rawImage != null) {
                capas[i].transform.SetSiblingIndex(i);
            }
            // Para SpriteRenderer: ajustar orden de dibujo (sortingOrder)
            if (capas[i].sprite != null) {
                capas[i].sprite.sortingOrder = i;
            }
        }
    }
}