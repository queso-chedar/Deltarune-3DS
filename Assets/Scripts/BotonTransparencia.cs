using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BotonTransparencia : MonoBehaviour
{
    public List<Button> botones;
    public float spacing = 200f; // distancia entre botones
    public float moveDuration = 0.2f; // velocidad de movimiento (suavizado)

    [Header("Alphas")]
    public float alphaSeleccionado = 1f;
    public float alphaVecino = 0.5f;
    public float alphaVecino2 = 0.25f;
    public float alphaOtros = 0f;
    public float fadeDuration = 0.2f;

    private int indiceActual = 0;

    void Start()
    {
        if (botones.Count > 0)
        {
            // Seleccionar el primero al inicio
            EventSystem.current.SetSelectedGameObject(botones[indiceActual].gameObject);
        }
    }

    void Update()
    {
        GameObject seleccionado = EventSystem.current.currentSelectedGameObject;
        if (seleccionado == null) return;

        // Actualizar índice seleccionado
        indiceActual = botones.FindIndex(b => b.gameObject == seleccionado);

        // Posicionar y hacer fade
        for (int i = 0; i < botones.Count; i++)
        {
            var img = botones[i].GetComponent<Image>();
            if (img == null) continue;

            // Distancia en índice con respecto al seleccionado
            int distancia = i - indiceActual;

            // Posición objetivo → el seleccionado en X=0 (centro), vecinos a ±spacing
            Vector3 targetPos = new Vector3(distancia * spacing, 0, 0);

            // Suavizar movimiento
            botones[i].transform.localPosition = Vector3.Lerp(
                botones[i].transform.localPosition,
                targetPos,
                Time.deltaTime / moveDuration
            );

            // Calcular alpha destino
            float targetAlpha = alphaOtros;
            if (Mathf.Abs(distancia) == 0) targetAlpha = alphaSeleccionado;
            else if (Mathf.Abs(distancia) == 1) targetAlpha = alphaVecino;
            else if (Mathf.Abs(distancia) == 2) targetAlpha = alphaVecino2;

            // Suavizar alpha
            Color c = img.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime / fadeDuration);
            img.color = c;
        }
    }
}
