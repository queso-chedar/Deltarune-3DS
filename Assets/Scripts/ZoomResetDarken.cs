using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZoomResetDarken : MonoBehaviour
{
    [Header("Sync with Shader")]
    [Tooltip("La misma velocidad (_Speed) que usas en tu shader ZoomTrail_Loop")]
    public float zoomSpeed = 0.2f;

    [Header("Overlay Settings")]
    [Tooltip("Un Image UI de pantalla completa, color negro, alpha inicial = 0")]
    public Image blackoutOverlay;
    [Tooltip("Duración total del parpadeo (fade in + fade out)")]
    public float blackoutDuration = 0.4f;
    [Tooltip("Proporción del tiempo dedicada al fade-in (resto es fade-out)")]
    [Range(0f,1f)]
    public float fadeInRatio = 0.2f;

    float prevT;

    void Start()
    {
        if (blackoutOverlay == null)
            Debug.LogError("Asigna el Overlay UI en el inspector.");

        // Asegúrate de que empiece transparente
        blackoutOverlay.color = new Color(0, 0, 0, 0);
        prevT = GetNormalizedTime();
    }

    void Update()
    {
        float t = GetNormalizedTime();
        // Cuando t retrocede (pasa de cerca de 1 a cerca de 0) significa reinicio
        if (t < prevT)
            StartCoroutine(DoBlackout());

        prevT = t;
    }

    // Normaliza Time.time al rango [0,1) según zoomSpeed
    float GetNormalizedTime()
    {
        return Mathf.Repeat(Time.time * zoomSpeed, 1f);
    }

    IEnumerator DoBlackout()
    {
        float halfTime = blackoutDuration * fadeInRatio;
        float fadeOutTime = blackoutDuration - halfTime;
        // Fade In
        for (float elapsed = 0f; elapsed < halfTime; elapsed += Time.deltaTime)
        {
            float a = Mathf.Lerp(0f, 1f, elapsed / halfTime);
            blackoutOverlay.color = new Color(0, 0, 0, a);
            yield return null;
        }
        blackoutOverlay.color = Color.black;

        // Fade Out
        for (float elapsed = 0f; elapsed < fadeOutTime; elapsed += Time.deltaTime)
        {
            float a = Mathf.Lerp(1f, 0f, elapsed / fadeOutTime);
            blackoutOverlay.color = new Color(0, 0, 0, a);
            yield return null;
        }
        blackoutOverlay.color = new Color(0, 0, 0, 0);
    }
}
