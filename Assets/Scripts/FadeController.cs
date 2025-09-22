using UnityEngine;

public class FadeController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    private bool isFading = false;
    private float targetAlpha;
    private float fadeSpeed;

    void OnEnable()
    {
        // Inicializamos fade al alpha actual
        targetAlpha = canvasGroup.alpha;
    }

    void Update()
    {
        if (isFading)
        {
            // Movemos alpha hacia targetAlpha usando Time.deltaTime
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

            // Si llegamos al objetivo, paramos
            if (Mathf.Approximately(canvasGroup.alpha, targetAlpha))
                isFading = false;
        }
    }

    // Fade suave
    public void FadeTo(float alpha)
    {
        targetAlpha = alpha;
        fadeSpeed = Mathf.Abs(canvasGroup.alpha - targetAlpha) / fadeDuration;
        isFading = true;
    }

    // Fade instantáneo
    public void SetAlphaInstant(float alpha)
    {
        canvasGroup.alpha = alpha;
        isFading = false;
    }
}
