using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PitchFader : MonoBehaviour
{
    [Header("Configuración de Fade")]
    [SerializeField] private float initialPitch = 1.0f;    // Pitch al inicio
    [SerializeField] private float targetPitch = 1.5f;     // Pitch al final
    [SerializeField] private float fadeTime = 5.0f;        // Duración del fade en segundos
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1); // Curva de interpolación

    private AudioSource audioSource;
    private float currentTime = 0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = initialPitch; // Fija el pitch inicial
    }

    private void Update()
    {
        if (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / fadeTime);
            float curveProgress = fadeCurve.Evaluate(progress); // Aplica la curva
            audioSource.pitch = Mathf.Lerp(initialPitch, targetPitch, curveProgress);
        }
    }

    // Opcional: Reiniciar el fade manualmente
    public void ResetFade()
    {
        currentTime = 0f;
        audioSource.pitch = initialPitch;
    }
}