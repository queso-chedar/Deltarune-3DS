using UnityEngine;
using System.Collections;

public class scr_Tween : MonoBehaviour
{
    [Header("Tween Settings")]
    public Vector3 targetPosition;      // Posición final
    public Vector3 targetScale = Vector3.one;   // Escala final
    public float targetRotation = 0f;   // Rotación Z final en grados

    public float duration = 2f;         // Tiempo
    public bool loop = false;           // Loop ping-pong
    public float delay = 0f;            // Delay inicial
    public EaseType easeType = EaseType.Linear;

    private Vector3 startPosition;
    private Vector3 startScale;
    private float startRotation;

    private Coroutine running;

    void Start()
    {
        startPosition = transform.position;
        startScale = transform.localScale;
        startRotation = transform.eulerAngles.z;

        if (running != null) StopCoroutine(running);
        running = StartCoroutine(TweenCoroutine());
    }

    private IEnumerator TweenCoroutine()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        do
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                t = Ease.Apply(t, easeType);

                // Posición
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                // Escala
                transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                // Rotación Z
                float rot = Mathf.LerpAngle(startRotation, targetRotation, t);
                transform.rotation = Quaternion.Euler(0, 0, rot);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Asegurar valores finales exactos
            transform.position = targetPosition;
            transform.localScale = targetScale;
            transform.rotation = Quaternion.Euler(0, 0, targetRotation);

            if (loop)
            {
                // Ping-pong: intercambiar valores iniciales y finales
                Vector3 tmpPos = startPosition; startPosition = targetPosition; targetPosition = tmpPos;
                Vector3 tmpScale = startScale; startScale = targetScale; targetScale = tmpScale;
                float tmpRot = startRotation; startRotation = targetRotation; targetRotation = tmpRot;
            }

        } while (loop);
    }

    public enum EaseType
    {
        Linear,
        QuadIn, QuadOut, QuadInOut,
        CubeIn, CubeOut, CubeInOut,
        QuartIn, QuartOut, QuartInOut,
        QuintIn, QuintOut, QuintInOut,
        SmoothStepIn, SmoothStepOut, SmoothStepInOut,
        SmootherStepIn, SmootherStepOut, SmootherStepInOut,
        SineIn, SineOut, SineInOut,
        BounceIn, BounceOut, BounceInOut,
        CircIn, CircOut, CircInOut,
        ExpoIn, ExpoOut, ExpoInOut,
        BackIn, BackOut, BackInOut,
        ElasticIn, ElasticOut, ElasticInOut
    }
}

public static class Ease
{
    private static readonly float PI = Mathf.PI;
    private static readonly float PI2 = Mathf.PI / 2f;

    private static readonly float B1 = 1f / 2.75f;
    private static readonly float B2 = 2f / 2.75f;
    private static readonly float B3 = 1.5f / 2.75f;
    private static readonly float B4 = 2.5f / 2.75f;
    private static readonly float B5 = 2.25f / 2.75f;
    private static readonly float B6 = 2.625f / 2.75f;

    private static readonly float ELASTIC_AMPLITUDE = 1f;
    private static readonly float ELASTIC_PERIOD = 0.4f;

    public static float Apply(float t, scr_Tween.EaseType ease)
    {
        switch (ease)
        {
            case scr_Tween.EaseType.Linear: return t;

            case scr_Tween.EaseType.QuadIn: return t * t;
            case scr_Tween.EaseType.QuadOut: return -t * (t - 2);
            case scr_Tween.EaseType.QuadInOut: return t <= .5f ? t * t * 2 : 1 - (--t) * t * 2;

            case scr_Tween.EaseType.CubeIn: return t * t * t;
            case scr_Tween.EaseType.CubeOut: return 1 + (--t) * t * t;
            case scr_Tween.EaseType.CubeInOut: return t <= .5f ? t * t * t * 4 : 1 + (--t) * t * t * 4;

            case scr_Tween.EaseType.QuartIn: return t * t * t * t;
            case scr_Tween.EaseType.QuartOut: return 1 - Mathf.Pow(t - 1, 4);
            case scr_Tween.EaseType.QuartInOut: return t <= .5f ? Mathf.Pow(t * 2, 4) / 2 : (1 - Mathf.Pow(t * 2 - 2, 4)) / 2 + .5f;

            case scr_Tween.EaseType.QuintIn: return Mathf.Pow(t, 5);
            case scr_Tween.EaseType.QuintOut: return Mathf.Pow(t - 1, 5) + 1;
            case scr_Tween.EaseType.QuintInOut: return t < 0.5f ? Mathf.Pow(t * 2, 5) / 2 : (Mathf.Pow(t * 2 - 2, 5) + 2) / 2;

            case scr_Tween.EaseType.SmoothStepIn: return 2 * SmoothStepInOut(t / 2);
            case scr_Tween.EaseType.SmoothStepOut: return 2 * SmoothStepInOut(t / 2 + 0.5f) - 1;
            case scr_Tween.EaseType.SmoothStepInOut: return t * t * (t * -2 + 3);

            case scr_Tween.EaseType.SmootherStepIn: return 2 * SmootherStepInOut(t / 2);
            case scr_Tween.EaseType.SmootherStepOut: return 2 * SmootherStepInOut(t / 2 + 0.5f) - 1;
            case scr_Tween.EaseType.SmootherStepInOut: return t * t * t * (t * (t * 6 - 15) + 10);

            case scr_Tween.EaseType.SineIn: return -Mathf.Cos(PI2 * t) + 1;
            case scr_Tween.EaseType.SineOut: return Mathf.Sin(PI2 * t);
            case scr_Tween.EaseType.SineInOut: return -(Mathf.Cos(PI * t) - 1) / 2;

            case scr_Tween.EaseType.BounceIn: return 1 - BounceOut(1 - t);
            case scr_Tween.EaseType.BounceOut: return BounceOut(t);
            case scr_Tween.EaseType.BounceInOut: return t < 0.5f ? (1 - BounceOut(1 - 2 * t)) / 2 : (1 + BounceOut(2 * t - 1)) / 2;

            case scr_Tween.EaseType.CircIn: return -(Mathf.Sqrt(1 - t * t) - 1);
            case scr_Tween.EaseType.CircOut: return Mathf.Sqrt(1 - (t - 1) * (t - 1));
            case scr_Tween.EaseType.CircInOut: return t <= .5f ? (Mathf.Sqrt(1 - t * t * 4) - 1) / -2 : (Mathf.Sqrt(1 - Mathf.Pow(t * 2 - 2, 2)) + 1) / 2;

            case scr_Tween.EaseType.ExpoIn: return Mathf.Pow(2, 10 * (t - 1));
            case scr_Tween.EaseType.ExpoOut: return -Mathf.Pow(2, -10 * t) + 1;
            case scr_Tween.EaseType.ExpoInOut: return t < .5f ? Mathf.Pow(2, 10 * (t * 2 - 1)) / 2 : (-Mathf.Pow(2, -10 * (t * 2 - 1)) + 2) / 2;

            case scr_Tween.EaseType.BackIn: return t * t * (2.70158f * t - 1.70158f);
            case scr_Tween.EaseType.BackOut: t -= 1; return 1 + t * t * (2.70158f * t + 1.70158f);
            case scr_Tween.EaseType.BackInOut:
                t *= 2;
                if (t < 1) return t * t * (2.70158f * t - 1.70158f) / 2;
                t -= 2;
                return (t * t * (2.70158f * t + 1.70158f) + 2) / 2;

            case scr_Tween.EaseType.ElasticIn:
                return -(Mathf.Pow(2, 10 * (t - 1)) * Mathf.Sin((t - 1.075f) * (2 * Mathf.PI) / ELASTIC_PERIOD));
            case scr_Tween.EaseType.ElasticOut:
                return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - 0.075f) * (2 * Mathf.PI) / ELASTIC_PERIOD) + 1;
            case scr_Tween.EaseType.ElasticInOut:
                if (t < 0.5f)
                    return -0.5f * (Mathf.Pow(2, 10 * (t * 2 - 1)) * Mathf.Sin(((t * 2 - 1) - 0.1125f) * (2 * Mathf.PI) / ELASTIC_PERIOD));
                return Mathf.Pow(2, -10 * (t * 2 - 1)) * Mathf.Sin(((t * 2 - 1) - 0.1125f) * (2 * Mathf.PI) / ELASTIC_PERIOD) * 0.5f + 1;

            default: return t;
        }
    }

    private static float SmoothStepInOut(float t)
    {
        return t * t * (t * -2 + 3);
    }

    private static float SmootherStepInOut(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float BounceOut(float t)
    {
        if (t < B1) return 7.5625f * t * t;
        if (t < B2) return 7.5625f * (t - B3) * (t - B3) + 0.75f;
        if (t < B4) return 7.5625f * (t - B5) * (t - B5) + 0.9375f;
        return 7.5625f * (t - B6) * (t - B6) + 0.984375f;
    }
}