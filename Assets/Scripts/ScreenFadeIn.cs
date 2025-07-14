using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFadeIn : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float fadeDuration = 2.0f;
    [SerializeField] private Color fadeColor = Color.black;

    [Header("Pantallas 3DS")]
    [SerializeField] private Canvas topScreenCanvas;     // Canvas pantalla superior
    [SerializeField] private Canvas bottomScreenCanvas;  // Canvas pantalla inferior

    private Image topScreenFadeImage;
    private Image bottomScreenFadeImage;
    private bool is3DModeActive = false;  // Control del modo 3D

    private void Awake()
    {
        SetupFadeForScreen(topScreenCanvas, out topScreenFadeImage);
        SetupFadeForScreen(bottomScreenCanvas, out bottomScreenFadeImage);
        
        // Ajuste inicial para el modo 2D (400x240)
        UpdateTopScreenFadeSize(is3DModeActive);
        
        StartCoroutine(FadeIn());
    }

    private void SetupFadeForScreen(Canvas targetCanvas, out Image fadeImage)
    {
        GameObject fadeImageGO = new GameObject("FadeImage");
        fadeImageGO.transform.SetParent(targetCanvas.transform, false);
        fadeImage = fadeImageGO.AddComponent<Image>();
        fadeImage.color = fadeColor;

        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    // Método para cambiar el tamaño del fade en la pantalla superior
    private void UpdateTopScreenFadeSize(bool is3DEnabled)
    {
        if (topScreenFadeImage == null) return;

        RectTransform rect = topScreenFadeImage.GetComponent<RectTransform>();
        if (is3DEnabled)
        {
            // Modo 3D: 800x240 (duplica el ancho)
            rect.localScale = new Vector3(2f, 1f, 1f); // Escala en X
        }
        else
        {
            // Modo 2D: 400x240 (escala normal)
            rect.localScale = Vector3.one;
        }
    }

    // Llamar este método al activar/desactivar el 3D
    public void Set3DMode(bool enabled)
    {
        is3DModeActive = enabled;
        UpdateTopScreenFadeSize(enabled);
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color startColor = fadeColor;
        Color endColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / fadeDuration);
            topScreenFadeImage.color = Color.Lerp(startColor, endColor, progress);
            bottomScreenFadeImage.color = Color.Lerp(startColor, endColor, progress);
            yield return null;
        }

        Destroy(topScreenFadeImage.gameObject);
        Destroy(bottomScreenFadeImage.gameObject);
    }
}
