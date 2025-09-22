using UnityEngine;
using UnityEngine.UI;

public class FileSelectorUI : MonoBehaviour
{
    [Header("Referencias Slots")]
    public RectTransform[] slots;  // Paneles padres
    public Image[] slotImages;    // Imágenes de fondo de cada slot (opcional)
    public Text[] selectedTexts;  // Textos ESPECÍFICOS que se oscurecerán (deben coincidir con el orden de los slots)

    [Header("Selector")]
    public RectTransform selector;
    public Vector2 selectorOffset = new Vector2(-30, 0);
    public float moveSpeed = 10f;

    [Header("Efectos Visuales")]
    public Color colorNormal = Color.white;
    public Color colorOscuro = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    public bool scaleEffect = true;
    public float selectedScale = 1.05f;

    [Header("Sonidos")]
    public AudioClip moveSound;
    public AudioClip selectSound;
    private AudioSource audioSource;

    private int currentIndex = 0;
    private Vector3 targetPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (slots.Length > 0)
        {
            targetPosition = slots[0].position + (Vector3)selectorOffset;
            UpdateVisuals();
        }
    }

    void Update()
    {
        HandleInput();
        selector.position = Vector3.Lerp(selector.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) Move(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) Move(1);
        else if (Input.GetKeyDown(KeyCode.Return)) ConfirmSelection();
    }

    void Move(int direction)
    {
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, slots.Length - 1);
        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            targetPosition = slots[currentIndex].position + (Vector3)selectorOffset;
            UpdateVisuals();
            PlaySound(moveSound);
        }
    }

    void UpdateVisuals()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            bool isSelected = (i == currentIndex);
            Color color = isSelected ? colorNormal : colorOscuro;

            // Oscurece imagen de fondo (si existe)
            if (slotImages != null && i < slotImages.Length && slotImages[i] != null)
                slotImages[i].color = color;

            // Oscurece texto específico (si existe)
            if (selectedTexts != null && i < selectedTexts.Length && selectedTexts[i] != null)
                selectedTexts[i].color = color;

            // Efecto de escala (opcional)
            if (scaleEffect)
                slots[i].localScale = Vector3.one * (isSelected ? selectedScale : 1f);
        }
    }

    public void ConfirmSelection()
    {
        Debug.Log("Archivo seleccionado: " + (currentIndex + 1));
        PlaySound(selectSound);
        // Aquí tu lógica de carga de archivo
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}