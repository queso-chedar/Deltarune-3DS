using UnityEngine;

public class Example : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public CanvasGroup canvasGroupDown;
    public float duracion = 2f; // tiempo total en segundos
    private float tiempo = 0f;
    public Canvas myCanvas;
    public Canvas myCanvasDown;
    void Start()
    {
        myCanvas.gameObject.SetActive(true);  // CORRECTO
        myCanvasDown.gameObject.SetActive(true);  // CORRECTO
    }

    void Update()
    {
        if (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion; // convierte tiempo en porcentaje (0 → 1)
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            canvasGroupDown.alpha = Mathf.Lerp(0f, 1f, t);
        }
    }
}
