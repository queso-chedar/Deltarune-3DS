using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

// Incluso yo Siento miedo genuino hacia este codigo. pofavor sigue con cuidado

// Ah y suerte si logras entenderlo.

public class DialogueSystem : MonoBehaviour
{
    [Header("Porfavor no toques esto no sirve de nada y rompe el codigo si lo haces")]
    public float spaceWidth = 30f;
    public Vector2 defaultImageOffset = new Vector2(0, 0);

    [System.Serializable]
    public class Dialogue
    {
        [TextArea(1, 4)]
        public string message;
        public Sprite headSprite;
        public bool skippable = true;
        public string eventTrigger;
        public Vector2 imageOffset = new Vector2(0, 0);  // Offset personalizado
        public Sprite customImage;  // Imagen personalizada para este diálogo (opcional)
    }

    [System.Serializable]
    public class Choice
    {
        public string optionText;
        public int nextDialogueIndex;
    }

    [System.Serializable]
    public class DialogueNode
    {
        public Dialogue[] dialogues;
        public Choice[] choices;
        public bool hasChoices;
    }

    [System.Serializable]
    public class ColorTag
    {
        public string tag;
        public Color color;
    }

    [System.Serializable]
    public class EmbeddedImage
    {
        public string tag;
        public Sprite image;
    }

    public DialogueNode[] dialogueNodes;
    public GameObject dialogueBoxPrefab;
    public GameObject choiceButtonPrefab;

    [Header("Text Settings")]
    public float textSpeed = 0.05f;
    public float punctuationPause = 0.2f;
    public float autoAdvanceDelay = 1.0f;

    [Header("Sound Settings")]
    public AudioClip[] typeSounds;
    public AudioClip choiceSound;
    public AudioClip punctuationSound;
    [Range(0.1f, 3.0f)] public float pitch = 1.0f;
    [Range(0.1f, 3.0f)] public float punctuationPitch = 1.0f;

    [Header("Performance Settings")]
    public int audioPoolSize = 5;
    public float letterSoundCooldown = 0.03f;

    [Header("Text Formatting")]
    public ColorTag[] colorTags;
    public EmbeddedImage[] embeddedImages;
    public GameObject imagePrefab;
    public float imageScale = 1.0f;

    public event Action<string> OnEventTriggered;

    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    private GameObject currentDialogueBox;
    private Text dialogueText;
    private Image headImage;
    private int currentNodeIndex = 0;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private bool inChoice = false;
    private List<GameObject> choiceButtons = new List<GameObject>();
    private string currentFullText = "";
    private bool waitingForAutoAdvance = false;
    private float lastSoundTime;

    private List<GameObject> activeImages = new List<GameObject>();
    private List<GameObject> imagePool = new List<GameObject>();

    void Start()
    {
        InitializeAudioPool();
        ShowDialogue();
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        if (canvas != null)
        {
            if (!canvas.pixelPerfect)
            {
                Debug.LogWarning("Se recomienda activar el Parametro 'Pixel Perfect' en el Inspector del Canvas para evitar problemas de suavizado.");
            }
        }
    }
    void InitializeAudioPool()
    {
        for (int i = 0; i < audioPoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0;
            source.bypassEffects = true;
            source.bypassListenerEffects = true;
            audioSourcePool.Add(source);
        }
    }

    void PlaySound(AudioClip clip, float soundPitch)
    {
        if (clip == null) return;

        if (Time.time - lastSoundTime < letterSoundCooldown) return;

        AudioSource availableSource = null;

        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                availableSource = source;
                break;
            }
        }

        if (availableSource == null && audioSourcePool.Count > 0)
        {
            availableSource = audioSourcePool[0];
        }

        if (availableSource != null)
        {
            availableSource.clip = clip;
            availableSource.pitch = soundPitch;
            availableSource.Play();
            lastSoundTime = Time.time;
        }
    }

    void Update()
    {
        if (inChoice || currentDialogueBox == null) return;
        if (inChoice) return;

        if (isTyping && GetCurrentDialogue() != null && GetCurrentDialogue().skippable &&
            (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Return)))
        {
            CompleteText();
        }
        else if (!isTyping && !waitingForAutoAdvance &&
                (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Return)))
        {
            NextDialogue();
        }
            Dialogue currentDialogue = GetCurrentDialogue();
        if (currentDialogue != null && !string.IsNullOrEmpty(currentDialogue.eventTrigger))
        {
            cebollaevento[] receptores = FindObjectsOfType<cebollaevento>();

            foreach (var receptor in receptores)
            {
                receptor.TryActivate(currentDialogue.eventTrigger);
            }

            // Limpia el trigger después de usarlo
            currentDialogue.eventTrigger = "";
        }
    }

    Dialogue GetCurrentDialogue()
    {
        if (currentNodeIndex < dialogueNodes.Length &&
            currentDialogueIndex < dialogueNodes[currentNodeIndex].dialogues.Length)
        {
            return dialogueNodes[currentNodeIndex].dialogues[currentDialogueIndex];
        }
        return null;
    }

    void ShowDialogue()
    {
        if (currentDialogueBox == null)
        {
            GameObject canvasObj = GameObject.Find("Canvas");

            if (canvasObj != null)
            {
                currentDialogueBox = Instantiate(dialogueBoxPrefab, canvasObj.transform);

                dialogueText = currentDialogueBox.GetComponentInChildren<Text>();
                dialogueText.supportRichText = true; // Habilitar texto enriquecido

                headImage = FindChildRecursive(currentDialogueBox.transform, "HeadImage").GetComponent<Image>();
            }
            else
            {
                Debug.LogError("No se encontró el objeto Canvas. Asegúrate de que exista en la escena.");
            }
        }
        Dialogue currentDialogue = GetCurrentDialogue();
        if (currentDialogue == null)
        {
            CloseDialogue();
            return;
        }

        currentFullText = currentDialogue.message; // Guardar el texto completo

        if (!string.IsNullOrEmpty(currentDialogue.eventTrigger))
        {
            if (OnEventTriggered != null)
                OnEventTriggered(currentDialogue.eventTrigger);
        }

        if (headImage != null && currentDialogue.headSprite != null)
        {
            headImage.sprite = currentDialogue.headSprite;
            headImage.enabled = true;
        }
        else if (headImage != null)
        {
            headImage.enabled = false;
        }

        StartCoroutine(TypeText(currentFullText));
    }

    Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = FindChildRecursive(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
    private IEnumerator TypeText(string fullText)
    {
        // ===== 1. DECLARACIONES INICIALES =====
        Dialogue currentDialogue = GetCurrentDialogue();
        Vector2 currentImageOffset = defaultImageOffset;
        List<object> tokens = new List<object>();
        System.Text.StringBuilder displayText = new System.Text.StringBuilder();
        int tokenIndex = 0;
        float currentTextWidth = 0f;

        // ===== 2. INICIALIZACIÓN =====
        isTyping = true;
        waitingForAutoAdvance = false;
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
        else
        {
            Debug.LogError("No se ha encontrado Un DialogueText. Haz olvidado Crearselo a tu Panel? O Haz olvidado crear un canvas en primer lugar???");
            yield break;
        }
        dialogueText.text = "";
        ReturnAllImagesToPool();
        activeImages.Clear();
        currentFullText = fullText;

        // ===== 3. CONFIGURACIÓN DE OFFSET =====
        if (currentDialogue != null && currentDialogue.imageOffset != null)
        {
            currentImageOffset = currentDialogue.imageOffset;
        }

        // ===== 4. PARSEO DE TEXTO =====
        tokens = ParseFormattedText(fullText, currentDialogue);

        // ===== 5. BUCLE PRINCIPAL =====
        while (tokenIndex < tokens.Count)
        {
            object token = tokens[tokenIndex];

            if (token is string)
            {
                string textPart = (string)token;
                for (int i = 0; i < textPart.Length; i++)
                {
                    char c = textPart[i];
                    displayText.Append(c);
                    dialogueText.text = displayText.ToString();

                    // Calcular ancho manualmente
                    CharacterInfo charInfo;
                    if (dialogueText.font.GetCharacterInfo(c, out charInfo, dialogueText.fontSize, dialogueText.fontStyle))
                    {
                        currentTextWidth += charInfo.advance;
                    }
                    else
                    {
                        currentTextWidth += dialogueText.fontSize * 0.5f;
                    }

                    PlayTypeSound(c);

                    float waitTime = textSpeed;
                    if (IsPunctuation(c))
                    {
                        waitTime += punctuationPause;
                        PlayPunctuationSound();
                    }

                    if (i < textPart.Length - 4)
                    {
                        yield return new WaitForSeconds(waitTime);
                    }
                    else
                    {
                        float accumulated = 0;
                        while (accumulated < waitTime)
                        {
                            accumulated += Time.deltaTime;
                            yield return null;
                        }
                    }

                    if (!isTyping)
                    {
                        yield break;
                    }
                }
            }
            else if (token is EmbeddedImageData)
            {
                EmbeddedImageData imgData = (EmbeddedImageData)token;
                GameObject imgObj = GetImageFromPool();

                // Configurar imagen
                imgObj.transform.SetParent(dialogueText.transform.parent);
                imgObj.transform.localScale = Vector3.one * imageScale;
                imgObj.GetComponent<Image>().sprite = imgData.sprite;
                imgObj.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(imgData.width, imgData.height);

                // Calcular posición MANUALMENTE
                Vector3 imgPosition = dialogueText.rectTransform.position;
                imgPosition.x += currentTextWidth + currentImageOffset.x;
                imgPosition.y += currentImageOffset.y;
                imgObj.transform.position = imgPosition;

                imgObj.SetActive(true);
                activeImages.Add(imgObj);

                // Avanzar ancho y agregar espacio
                currentTextWidth += imgData.width;
                displayText.Append(" ");
                dialogueText.text = displayText.ToString();

                yield return new WaitForSeconds(textSpeed * 2);
            }

            tokenIndex++;
        }

        // ===== 6. FINALIZACIÓN =====
        isTyping = false;

        if (currentDialogue == null)
        {
            CloseDialogue();
            yield break;
        }

        if (!currentDialogue.skippable)
        {
            waitingForAutoAdvance = true;
            yield return new WaitForSeconds(autoAdvanceDelay);
            waitingForAutoAdvance = false;
            NextDialogue();
        }
        else if (ShouldShowChoices())
        {
            yield return new WaitForSeconds(0.2f);
            ShowChoices();
        }
    }

    private bool ShouldShowChoices()
    {
        return currentDialogueIndex == dialogueNodes[currentNodeIndex].dialogues.Length - 1 &&
               dialogueNodes[currentNodeIndex].hasChoices &&
               dialogueNodes[currentNodeIndex].choices != null &&
               dialogueNodes[currentNodeIndex].choices.Length > 0;
    }
    private class EmbeddedImageData
    {
        public Sprite sprite;
        public float width;
        public float height; // Nuevo campo
    }
    private List<object> ParseFormattedText(string text, Dialogue currentDialogue)
    {
        List<object> tokens = new List<object>();
        int currentIndex = 0;

        while (currentIndex < text.Length)
        {
            int tagStart = text.IndexOf('{', currentIndex);
            if (tagStart == -1)
            {
                tokens.Add(text.Substring(currentIndex));
                break;
            }

            if (tagStart > currentIndex)
            {
                tokens.Add(text.Substring(currentIndex, tagStart - currentIndex));
            }

            int tagEnd = text.IndexOf('}', tagStart);
            if (tagEnd == -1)
            {
                tokens.Add(text.Substring(tagStart));
                break;
            }

            string tagContent = text.Substring(tagStart + 1, tagEnd - tagStart - 1);

            // Prioridad: Imagen personalizada del diálogo
            if (currentDialogue != null && currentDialogue.customImage != null)
            {
                tokens.Add(new EmbeddedImageData
                {
                    sprite = currentDialogue.customImage,
                    width = currentDialogue.customImage.rect.width * imageScale,
                    height = currentDialogue.customImage.rect.height * imageScale
                });
            }
            else
            {
                // Buscar en imágenes embebidas globales
                foreach (EmbeddedImage embeddedImg in embeddedImages)
                {
                    if (tagContent == embeddedImg.tag)
                    {
                        tokens.Add(new EmbeddedImageData
                        {
                            sprite = embeddedImg.image,
                            width = embeddedImg.image.rect.width * imageScale,
                            height = embeddedImg.image.rect.height * imageScale
                        });
                        break;
                    }
                }
            }

            currentIndex = tagEnd + 1;
        }

        return tokens;
    }

    private void UpdateImagePositions(string text)
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueText.rectTransform);

        // 1. Calcular posición base del texto
        Vector3 textStartPos = dialogueText.rectTransform.position;
        float baseY = textStartPos.y;

        // 2. Calcular offset vertical basado en alineación
        float yOffset = 0;
        switch (dialogueText.alignment)
        {
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperRight:
                yOffset = -dialogueText.preferredHeight * 0.5f;
                break;
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                yOffset = 0;
                break;
            case TextAnchor.LowerLeft:
            case TextAnchor.LowerCenter:
            case TextAnchor.LowerRight:
                yOffset = dialogueText.preferredHeight * 0.5f;
                break;
        }
        yOffset += dialogueText.fontSize * 0.5f + 5f;

        // 3. Procesar el texto para encontrar espacios de imágenes (sin TextGenerator)
        int imageIndex = 0;
        float currentWidth = 0;
        int currentLine = 0;
        float lineHeight = dialogueText.fontSize * dialogueText.lineSpacing;

        for (int i = 0; i < text.Length; i++)
        {
            // Manejo de saltos de línea
            if (text[i] == '\n')
            {
                currentWidth = 0;
                currentLine++;
                continue;
            }

            // Detectar inicio de tag <space>
            if (i + 7 < text.Length && text.Substring(i, 7) == "<space=")
            {
                // Extraer valor del espacio
                int endIndex = text.IndexOf('>', i);
                if (endIndex > i)
                {
                    string spaceValue = text.Substring(i + 7, endIndex - i - 7);
                    float spaceWidth;
                    if (float.TryParse(spaceValue, out spaceWidth))
                    {
                        // Posicionar imagen si existe
                        if (imageIndex < activeImages.Count)
                        {
                            // Calcular posición
                            Vector2 imgPos = new Vector2(
                                textStartPos.x + currentWidth + spaceWidth / 2,
                                baseY + yOffset - (currentLine * lineHeight)
                            );

                            // Aplicar posición
                            activeImages[imageIndex].transform.position = imgPos;

                            imageIndex++;
                        }

                        // Avanzar el ancho actual
                        currentWidth += spaceWidth;
                    }

                    // Saltar al final del tag
                    i = endIndex;
                    continue;
                }
            }

            // Procesar caracter normal
            CharacterInfo charInfo;
            if (dialogueText.font.GetCharacterInfo(text[i], out charInfo, dialogueText.fontSize, dialogueText.fontStyle))
            {
                currentWidth += charInfo.advance;
            }
            else
            {
                currentWidth += dialogueText.fontSize * 0.5f;
            }
        }

        // Posicionar imágenes restantes al final
        while (imageIndex < activeImages.Count)
        {
            activeImages[imageIndex].transform.position = new Vector3(
                textStartPos.x + currentWidth,
                baseY + yOffset - (currentLine * lineHeight),
                0
            );
            imageIndex++;
        }
    }

    void PlayTypeSound(char c)
    {
        if (IsAlphabetic(c) && typeSounds != null && typeSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, typeSounds.Length);
            PlaySound(typeSounds[randomIndex], pitch);
        }
    }
    void CompleteText()
    {
        // ===== 1. DETENER CORRUTINA =====
        StopAllCoroutines();

        // ===== 2. MOSTRAR TEXTO COMPLETO =====
        dialogueText.text = currentFullText;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueText.rectTransform);

        // ===== 3. CONFIGURACIÓN =====
        Dialogue currentDialogue = GetCurrentDialogue();
        Vector2 currentImageOffset = defaultImageOffset;
        if (currentDialogue != null && currentDialogue.imageOffset != null)
        {
            currentImageOffset = currentDialogue.imageOffset;
        }

        // ===== 4. PROCESAR TOKENS =====
        List<object> tokens = ParseFormattedText(currentFullText, currentDialogue);
        System.Text.StringBuilder displayText = new System.Text.StringBuilder();
        float currentWidth = 0f;

        for (int i = 0; i < tokens.Count; i++)
        {
            object token = tokens[i];

            if (token is string)
            {
                string textPart = (string)token;
                displayText.Append(textPart);
                currentWidth = CalculateTextWidth(textPart);
            }
            else if (token is EmbeddedImageData)
            {
                // SOLUCIÓN: Declaraciones únicas sin conflicto de ámbito
                var imgData = (EmbeddedImageData)token;
                var imgObj = GetImageFromPool();

                if (imgObj != null)
                {
                    // Configurar imagen
                    imgObj.transform.SetParent(dialogueText.transform.parent);
                    imgObj.transform.localScale = Vector3.one * imageScale;
                    imgObj.GetComponent<Image>().sprite = imgData.sprite;
                    imgObj.GetComponent<RectTransform>().sizeDelta =
                        new Vector2(imgData.width, imgData.height);

                    // Posicionamiento preciso
                    Vector3 imgPosition = dialogueText.rectTransform.position;
                    imgPosition.x += currentWidth + currentImageOffset.x;
                    imgPosition.y += currentImageOffset.y;
                    imgObj.transform.position = imgPosition;

                    imgObj.SetActive(true);
                    activeImages.Add(imgObj);

                    // Espacio y ancho
                    displayText.Append(" ");
                    currentWidth += imgData.width;
                }
            }
        }

        // ===== 5. ACTUALIZAR TEXTO FINAL =====
        dialogueText.text = displayText.ToString();

        // ===== 6. FINALIZAR ESTADO =====
        isTyping = false;
        waitingForAutoAdvance = false;

        // ===== 7. MANEJAR OPCIONES =====
        if (ShouldShowChoices())
        {
            ShowChoices();
        }
    }
    private float CalculateTextWidth(string text)
    {
        float width = 0f;
        foreach (char c in text)
        {
            CharacterInfo charInfo;
            if (dialogueText.font.GetCharacterInfo(c, out charInfo, dialogueText.fontSize, dialogueText.fontStyle))
            {
                width += charInfo.advance;
            }
            else
            {
                width += dialogueText.fontSize * 0.5f;
            }
        }
        return width;
    }
    bool IsAlphabetic(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||
               c == 'á' || c == 'é' || c == 'í' || c == 'ó' || c == 'ú' ||
               c == 'Á' || c == 'É' || c == 'Í' || c == 'Ó' || c == 'Ú' ||
               c == 'ñ' || c == 'Ñ' || c == '1' || c == '2' || c == '3' ||
               c == '4' || c == '5' || c == '6' || c == '7' || c == '8' ||
               c == '9' || c == '0' || c == '[' || c == ']';
               // estas maracuyadas son los signos o letras que no sonaran
    }

    void PlayPunctuationSound()
    {
        PlaySound(punctuationSound, punctuationPitch);
    }

    void PlayChoiceSound()
    {
        PlaySound(choiceSound, pitch);
    }

    bool IsPunctuation(char c)
    {
        return c == ',' || c == '.' || c == '?' || c == '!' || c == ';' || c == ':';
    }

    void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentNodeIndex >= dialogueNodes.Length)
        {
            CloseDialogue();
            return;
        }

        if (currentDialogueIndex < dialogueNodes[currentNodeIndex].dialogues.Length)
        {
            ShowDialogue();
        }
        else
        {
            if (!dialogueNodes[currentNodeIndex].hasChoices)
            {
                currentNodeIndex++;
                currentDialogueIndex = 0;

                if (currentNodeIndex < dialogueNodes.Length)
                {
                    ShowDialogue();
                }
                else
                {
                    CloseDialogue();
                }
            }
            else
            {
                if (dialogueNodes[currentNodeIndex].choices != null &&
                    dialogueNodes[currentNodeIndex].choices.Length > 0)
                {
                    ShowChoices();
                }
                else
                {
                    CloseDialogue();
                }
            }
        }
    }

    void ShowChoices()
    {
        if (dialogueNodes[currentNodeIndex].choices == null ||
            dialogueNodes[currentNodeIndex].choices.Length == 0)
        {
            CloseDialogue();
            return;
        }

        inChoice = true;

        GameObject choicesPanel = new GameObject("ChoicesPanel");
        choicesPanel.transform.SetParent(currentDialogueBox.transform);
        RectTransform rt = choicesPanel.AddComponent<RectTransform>();
        VerticalLayoutGroup layout = choicesPanel.AddComponent<VerticalLayoutGroup>();

        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 10;
        layout.padding = new RectOffset(10, 10, 10, 10);

        rt.anchorMin = new Vector2(0.5f, 0);
        rt.anchorMax = new Vector2(0.5f, 0);
        rt.pivot = new Vector2(0.5f, 0);
        rt.anchoredPosition = new Vector2(0, 20);
        rt.sizeDelta = new Vector2(300, 100);

        ContentSizeFitter sizeFitter = choicesPanel.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        for (int i = 0; i < dialogueNodes[currentNodeIndex].choices.Length; i++)
        {
            GameObject button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            choiceButtons.Add(button);

            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = dialogueNodes[currentNodeIndex].choices[i].optionText;

            ContentSizeFitter buttonFitter = button.AddComponent<ContentSizeFitter>();
            buttonFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            buttonFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            int choiceIndex = i;
            button.GetComponent<Button>().onClick.AddListener(() => SelectChoice(choiceIndex));
        }
    }

    void SelectChoice(int choiceIndex)
    {
        PlayChoiceSound();

        foreach (GameObject button in choiceButtons)
        {
            Destroy(button);
        }
        choiceButtons.Clear();

        inChoice = false;

        int nextNode = dialogueNodes[currentNodeIndex].choices[choiceIndex].nextDialogueIndex;

        if (nextNode >= 0 && nextNode < dialogueNodes.Length)
        {
            currentNodeIndex = nextNode;
            currentDialogueIndex = 0;
            ShowDialogue();
        }
        else
        {
            CloseDialogue();
        }
    }

    void CloseDialogue()
    {
        // 1. Detener todas las corrutinas primero
        StopAllCoroutines();

        // 2. Limpiar diálogo principal
        if (currentDialogueBox != null)
        {
            Destroy(currentDialogueBox);
            currentDialogueBox = null;
        }

        // 3. Limpiar botones de elección
        foreach (GameObject button in choiceButtons)
        {
            if (button != null) Destroy(button);
        }
        choiceButtons.Clear();

        // 4. Limpiar imágenes de manera segura
        ReturnAllImagesToPool();

        // 5. Resetear variables de estado
        isTyping = false;
        inChoice = false;
        waitingForAutoAdvance = false;
    }

    GameObject GetImageFromPool()
    {
        // Primero limpiar el pool de objetos destruidos
        imagePool.RemoveAll(item => item == null);

        foreach (GameObject img in imagePool)
        {
            if (img != null && !img.activeInHierarchy)
            {
                img.SetActive(true);
                return img;
            }
        }

        // Crear nueva imagen si no hay disponibles
        GameObject newImg = Instantiate(imagePrefab);
        newImg.SetActive(false);
        imagePool.Add(newImg);
        return newImg;
    }

    void ReturnAllImagesToPool()
    {
        // Recorrer en reversa para evitar problemas de índice
        for (int i = activeImages.Count - 1; i >= 0; i--)
        {
            if (activeImages[i] != null) // Verificar que el objeto existe
            {
                activeImages[i].SetActive(false);
                activeImages[i].transform.SetParent(null); // Desvincular de cualquier padre
            }
            activeImages.RemoveAt(i); // Remover de la lista siempre
        }
    }
}