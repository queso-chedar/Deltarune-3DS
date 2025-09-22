using UnityEngine;

public class AfterimageTrail : MonoBehaviour
{
    [Header("Target")]
    public SpriteRenderer targetRenderer;

    [Header("Trail Density")]
    public int copies = 4; // cantidad de sprites "multiplicados" por spawn
    public float initialOffsetDistance = 0f; // distancia entre las copias al generarse

    [Header("Appearance")]
    public Color trailColor = new Color(1f, 1f, 1f, 0.5f); 
    public float fadeSpeed = 0.5f; // alfa por segundo

    [Header("Movement")]
    public Vector2 trailDirection = Vector2.left; 
    public float maxTravelDistance = 1f; 

    [Header("Spawn")]
    public float spawnInterval = 0.05f;

    float timer;

    void Reset()
    {
        targetRenderer = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        if (targetRenderer == null) targetRenderer = GetComponent<SpriteRenderer>();
        if (targetRenderer == null)
            Debug.LogWarning("AfterimageTrail: No SpriteRenderer asignado ni encontrado en el mismo GameObject.");
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnCopies();
            timer = spawnInterval;
        }
    }

    void SpawnCopies()
    {
        if (targetRenderer == null) return;

        int safeCopies = Mathf.Max(1, copies);
        float initialAlpha = Mathf.Clamp01(trailColor.a);
        float safeFade = Mathf.Max(fadeSpeed, 0.0001f);

        float lifetime = initialAlpha / safeFade;
        float moveSpeed = (lifetime > 0f) ? (maxTravelDistance / lifetime) : 0f;
        Vector2 dir = trailDirection.normalized;

        float step = (safeCopies > 1) ? initialOffsetDistance / (safeCopies - 1) : 0f;

        for (int i = 0; i < safeCopies; i++)
        {
            GameObject after = new GameObject("Afterimage");
            SpriteRenderer sr = after.AddComponent<SpriteRenderer>();

            sr.sprite = targetRenderer.sprite;
            sr.flipX = targetRenderer.flipX;
            sr.flipY = targetRenderer.flipY;
            sr.sortingLayerID = targetRenderer.sortingLayerID;
            sr.sortingOrder = targetRenderer.sortingOrder - 1;
            sr.color = trailColor;

            float offsetAlong = i * step;
            Vector3 initialPos = targetRenderer.transform.position + (Vector3)(dir * offsetAlong);
            after.transform.position = initialPos;
            after.transform.rotation = targetRenderer.transform.rotation;
            after.transform.localScale = targetRenderer.transform.lossyScale;

            AfterimageMovingFade amf = after.AddComponent<AfterimageMovingFade>();
            amf.fadeSpeed = safeFade;
            amf.moveDirection = dir;
            amf.moveSpeed = moveSpeed;
            amf.initialAlpha = initialAlpha;

            Destroy(after, lifetime + 0.2f);
        }
    }
}

public class AfterimageMovingFade : MonoBehaviour
{
    [HideInInspector] public float fadeSpeed = 0.5f;
    [HideInInspector] public Vector2 moveDirection = Vector2.left;
    [HideInInspector] public float moveSpeed = 0.5f;
    [HideInInspector] public float initialAlpha = 0.5f;

    SpriteRenderer sr;
    Color col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) { Destroy(gameObject); return; }
        col = sr.color;
        col.a = initialAlpha;
        sr.color = col;
    }

    void Update()
    {
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        col.a -= fadeSpeed * Time.deltaTime;
        sr.color = col;

        if (col.a <= 0f)
            Destroy(gameObject);
    }
}