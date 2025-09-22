using UnityEngine;
using UnityEngine.UI;

public class UITextTrail : MonoBehaviour
{
    [Header("Target")]
    public Text targetText; // Texto principal (UI clásico)

    [Header("Trail Settings")]
    public float spawnInterval = 0.05f;
    public int copies = 3; 
    [Range(0f, 1f)] public float startAlpha = 0.5f; // Transparencia inicial
    public float fadeSpeed = 2f;
    public float maxTravelDistance = 20f;
    public Vector2 trailDirection = new Vector2(5f, 0f); // en píxeles

    private float timer;

    void Reset()
    {
        targetText = GetComponent<Text>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnAfterimage();
            timer = spawnInterval;
        }
    }

    void SpawnAfterimage()
    {
        if (targetText == null) return;

        for (int i = 0; i < copies; i++)
        {
            // Crear objeto duplicado en el mismo Canvas
            GameObject ghostObj = new GameObject("TextAfterimage");
            ghostObj.transform.SetParent(targetText.transform.parent);
            ghostObj.transform.SetSiblingIndex(targetText.transform.GetSiblingIndex());

            Text ghostText = ghostObj.AddComponent<Text>();

            // Copiar propiedades visuales del texto original
            ghostText.text = targetText.text;
            ghostText.font = targetText.font;
            ghostText.fontSize = targetText.fontSize;
            ghostText.fontStyle = targetText.fontStyle;
            ghostText.alignment = targetText.alignment;
            ghostText.raycastTarget = false;

            // Color con alpha inicial definido
            Color startColor = targetText.color;
            startColor.a = startAlpha;
            ghostText.color = startColor;

            // Posición inicial con offset
            RectTransform rt = ghostObj.GetComponent<RectTransform>();
            RectTransform originalRT = targetText.GetComponent<RectTransform>();
            rt.sizeDelta = originalRT.sizeDelta;
            rt.anchorMin = originalRT.anchorMin;
            rt.anchorMax = originalRT.anchorMax;
            rt.pivot = originalRT.pivot;
            rt.localScale = originalRT.localScale;
            rt.position = originalRT.position + (Vector3)(trailDirection * i);

            // Script de fade/movimiento
            UITextAfterimageFade fade = ghostObj.AddComponent<UITextAfterimageFade>();
            fade.fadeSpeed = fadeSpeed;
            fade.moveDirection = trailDirection.normalized;
            fade.moveSpeed = maxTravelDistance * fadeSpeed;
        }
    }
}

public class UITextAfterimageFade : MonoBehaviour
{
    [HideInInspector] public float fadeSpeed = 2f;
    [HideInInspector] public Vector2 moveDirection = Vector2.zero;
    [HideInInspector] public float moveSpeed = 50f;

    private Text textComp;
    private Color color;

    void Start()
    {
        textComp = GetComponent<Text>();
        color = textComp.color;
    }

    void Update()
    {
        // Mover en la dirección indicada
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        // Desvanecer
        color.a -= fadeSpeed * Time.deltaTime;
        textComp.color = color;

        if (color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
