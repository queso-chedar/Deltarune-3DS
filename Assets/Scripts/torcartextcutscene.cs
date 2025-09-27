
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class torcartextcutscene : MonoBehaviour
{
    [Header("Configuración básica")]
    public GameObject dialogueBoxPrefab;
    public Dialogue[] dialogues;

    [System.Serializable]
    public class DialogueEventConfig
    {
        public string eventTrigger;
        [Tooltip("Activar evento al FINAL del diálogo")]
        public bool triggerAtEnd;
        [Tooltip("Retraso adicional para este evento")]
        public float eventDelay;
    }

    [System.Serializable]
    public class Dialogue
    {
        [TextArea(1, 4)] public string message;
        public Sprite headSprite;
        public bool skippable = true;
        public float autoAdvanceDelay = 1f;
        public float startDelay = 0f;
        [Header("Eventos")]
        public List<DialogueEventConfig> eventTriggers = new List<DialogueEventConfig>();
    }

    public delegate void DialogueEvent(string eventTrigger);
    public event DialogueEvent OnEventTriggered;

    [Header("Texto / Tipeo")]
    public float textSpeed = 0.05f;
    public float punctuationPause = 0.2f;
    public int textUpdateBatchSize = 3;
    public bool disableLayoutWhileTyping = true;
    public bool lowPerfMode = false;

    [Header("Audio")]
    public AudioClip[] typeSounds;
    public int audioPoolSize = 5;
    public float letterSoundCooldown = 0.03f;

    GameObject cachedCanvasObj;
    public GameObject currentDialogueBox;
    Text dialogueText;
    Image headImage;

    int currentDialogueIndex = 0;
    bool isTyping = false;
    string currentFullText = "";
    bool waitingForAutoAdvance = false;

    List<AudioSource> audioSourcePool = new List<AudioSource>();
    Dictionary<float, WaitForSeconds> waitCache = new Dictionary<float, WaitForSeconds>();
    Font cachedFont;
    int cachedFontSize;
    FontStyle cachedFontStyle;
    Dictionary<char, float> charAdvanceCache = new Dictionary<char, float>();

    List<ContentSizeFitter> cachedFitters;
    List<HorizontalLayoutGroup> cachedHLayouts;
    List<VerticalLayoutGroup> cachedVLayouts;
    List<Outline> cachedOutlines;
    GraphicRaycaster cachedRaycaster;

    Coroutine typingCoroutine = null;
    int lastTypeSoundIndex = -1;
    float lastSoundTime = -10f;

    public void Start()
    {
        InitializeAudioPool();
        cachedCanvasObj = GameObject.Find("CanvasPrefab");
        if (cachedCanvasObj == null) Debug.LogWarning("CanvasPrefab no encontrado.");
        PreloadFontCharacters();
    }

    // ---- NEW: input handling restored ----
    void Update()
    {
        if (currentDialogueBox == null) return;

        // If typing: allow skipping/complete with X, B, Return
        if (isTyping)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Return))
            {
                // Only allow skip if current dialogue is skippable
                if (currentDialogueIndex < dialogues.Length && dialogues[currentDialogueIndex].skippable)
                {
                    CompleteText();
                }
                else
                {
                    CompleteText();
                }
            }
        }
        else
        {
            // Not typing: advance with Z or Return or A
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.A))
            {
                NextDialogue();
            }
        }
    }
    // ---------------------------------------

    void InitializeAudioPool()
    {
        for (int i = 0; i < Mathf.Max(1, audioPoolSize); i++)
        {
            var s = gameObject.AddComponent<AudioSource>();
            s.playOnAwake = false;
            s.spatialBlend = 0;
            s.bypassEffects = true;
            audioSourcePool.Add(s);
        }
    }

    void PreloadFontCharacters()
    {
        if (dialogueBoxPrefab == null) return;
        var prefabText = dialogueBoxPrefab.GetComponentInChildren<Text>();
        if (prefabText == null) return;
        cachedFont = prefabText.font;
        cachedFontSize = prefabText.fontSize;
        cachedFontStyle = prefabText.fontStyle;

        HashSet<char> chars = new HashSet<char>();
        if (dialogues != null)
        {
            foreach (var d in dialogues)
                if (!string.IsNullOrEmpty(d.message))
                    foreach (var c in d.message) chars.Add(c);
        }

        string extras = "áéíóúÁÉÍÓÚñÑ.,!?;:()[]{}\"'ºª0123456789 ";
        foreach (var c in extras) chars.Add(c);
        for (int i = 32; i <= 126; i++) chars.Add((char)i);

        if (cachedFont != null)
        {
            foreach (var c in chars)
                cachedFont.RequestCharactersInTexture(c.ToString(), Mathf.Max(1, cachedFontSize), cachedFontStyle);
            Canvas.ForceUpdateCanvases();
        }
    }

    public void ShowDialogue()
    {
        if (dialogues == null || dialogues.Length == 0) return;
        if (currentDialogueIndex >= dialogues.Length) return;
        StartCoroutine(ShowDialogueWithDelay());
    }


    IEnumerator ShowDialogueWithDelay()
    {
        int index = currentDialogueIndex;
        if (dialogues == null || dialogues.Length == 0) yield break;
        if (index < 0 || index >= dialogues.Length) yield break;

        if (string.IsNullOrEmpty(dialogues[index].message))
        {
            NextDialogue();
            yield break;
        }

        var d = dialogues[index];

        if (d.startDelay > 0)
        {
            var w = GetWFS(d.startDelay);
            if (w != null) yield return w; else yield return null;
        }

        if (currentDialogueIndex != index) yield break;

        if (currentDialogueBox == null)
        {
            if (cachedCanvasObj == null) { Debug.LogError("No Canvas para instanciar diálogo."); yield break; }
            currentDialogueBox = Instantiate(dialogueBoxPrefab, cachedCanvasObj.transform);

            dialogueText = currentDialogueBox.GetComponentInChildren<Text>();
            if (dialogueText != null)
            {
                dialogueText.supportRichText = true;
                cachedFont = dialogueText.font;
                cachedFontSize = dialogueText.fontSize;
                cachedFontStyle = dialogueText.fontStyle;
                charAdvanceCache.Clear();
            }

            var headT = currentDialogueBox.transform.Find("HeadImage");
            if (headT != null) headImage = headT.GetComponent<Image>();

            CacheLayoutComponents();
        }

        currentFullText = d.message;
        if (headImage != null) { headImage.sprite = d.headSprite; headImage.enabled = (d.headSprite != null); }

        foreach (var ev in d.eventTriggers)
        {
            if (!ev.triggerAtEnd) StartCoroutine(TriggerEventWithDelay(ev));
        }

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(currentFullText, index));
    }

    IEnumerator TriggerEventWithDelay(DialogueEventConfig ev)
    {
        if (ev == null) yield break;
        if (ev.eventDelay > 0) { var w = GetWFS(ev.eventDelay); if (w != null) yield return w; else yield return null; }
        if (OnEventTriggered != null) OnEventTriggered(ev.eventTrigger);
    }

    IEnumerator TypeText(string fullText, int dialogueIndex)
    {
        List<object> tokens = new List<object>();
        if (!string.IsNullOrEmpty(fullText)) tokens.Add(fullText);

        var d = dialogues[dialogueIndex];
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        isTyping = true;
        waitingForAutoAdvance = false;

        if (dialogueText != null) dialogueText.text = "";
        currentFullText = fullText;

        if (disableLayoutWhileTyping) ToggleLayoutAndRaycaster(false);

        foreach (object token in tokens)
        {
            string textPart = token as string;
            if (string.IsNullOrEmpty(textPart)) continue;

            int pending = 0;
            for (int i = 0; i < textPart.Length; i++)
            {
                char c = textPart[i];
                sb.Append(c);
                pending++;
                float adv = GetCharAdvance(c);

                if (!lowPerfMode && typeSounds != null && typeSounds.Length > 0 && (i % 2 == 0))
                    PlayTypeSoundRoundRobin();

                float waitTime = textSpeed;
                if (IsPunctuation(c)) waitTime += punctuationPause;

                if (pending >= Mathf.Max(1, textUpdateBatchSize) || IsPunctuation(c))
                {
                    if (dialogueText != null) dialogueText.text = sb.ToString();
                    pending = 0;
                }

                var wfs = GetWFS(waitTime);
                if (wfs != null) yield return wfs; else yield return null;

                if (!isTyping)
                {
                    if (disableLayoutWhileTyping) ToggleLayoutAndRaycaster(true);
                    yield break;
                }
            }

            if (sb.Length > 0 && dialogueText != null) dialogueText.text = sb.ToString();
        }

        isTyping = false;
        typingCoroutine = null;

        if (disableLayoutWhileTyping) ToggleLayoutAndRaycaster(true);

        /* broken idk how to fix
        if (!d.skippable)
        {
            waitingForAutoAdvance = true;
            var w = GetWFS(d.autoAdvanceDelay); if (w != null) yield return w; else yield return null;
            waitingForAutoAdvance = false;
            NextDialogue();
        }
        */
    }

    float GetCharAdvance(char c)
    {
        float adv;
        if (charAdvanceCache.TryGetValue(c, out adv)) return adv;
        CharacterInfo ci;
        if (cachedFont != null && cachedFont.GetCharacterInfo(c, out ci, cachedFontSize, cachedFontStyle)) adv = ci.advance;
        else adv = cachedFontSize > 0 ? cachedFontSize * 0.5f : 6f;
        charAdvanceCache[c] = adv;
        return adv;
    }

    WaitForSeconds GetWFS(float t)
    {
        if (t <= 0f) return null;
        WaitForSeconds w;
        if (!waitCache.TryGetValue(t, out w)) { w = new WaitForSeconds(t); waitCache[t] = w; }
        return w;
    }

    void PlayTypeSoundRoundRobin()
    {
        if (typeSounds == null || typeSounds.Length == 0) return;
        if (Time.time - lastSoundTime < letterSoundCooldown) return;
        lastTypeSoundIndex = (lastTypeSoundIndex + 1) % typeSounds.Length;
        PlaySound(typeSounds[lastTypeSoundIndex]);
        lastSoundTime = Time.time;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        foreach (var s in audioSourcePool)
            if (!s.isPlaying) { s.clip = clip; s.Play(); return; }
    }

    bool IsPunctuation(char c) { return c == ',' || c == '.' || c == '?' || c == '!' || c == ';' || c == ':'; }

    public void CompleteText()
    {
        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); typingCoroutine = null; }
        if (dialogueText != null) dialogueText.text = currentFullText;
        isTyping = false;
        waitingForAutoAdvance = false;
        if (disableLayoutWhileTyping) ToggleLayoutAndRaycaster(true);
    }

    public void NextDialogue()
    {
        int idx = currentDialogueIndex;
        if (idx < dialogues.Length)
        {
            var curr = dialogues[idx];
            foreach (var ev in curr.eventTriggers)
                if (ev.triggerAtEnd) StartCoroutine(TriggerEventWithDelay(ev));
            currentDialogueIndex++;
            if (currentDialogueIndex < dialogues.Length) ShowDialogue();
            else CloseDialogue();
        }
    }

    void CloseDialogue()
    {
        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); typingCoroutine = null; }
        if (currentDialogueBox != null) { Destroy(currentDialogueBox); currentDialogueBox = null; }
        isTyping = false; waitingForAutoAdvance = false;
    }

    void CacheLayoutComponents()
    {
        cachedFitters = new List<ContentSizeFitter>();
        cachedHLayouts = new List<HorizontalLayoutGroup>();
        cachedVLayouts = new List<VerticalLayoutGroup>();
        cachedOutlines = new List<Outline>();
        cachedRaycaster = null;
        if (currentDialogueBox == null) return;

        var fitters = currentDialogueBox.GetComponentsInChildren<ContentSizeFitter>(true);
        cachedFitters.AddRange(fitters);
        var hgs = currentDialogueBox.GetComponentsInChildren<HorizontalLayoutGroup>(true);
        cachedHLayouts.AddRange(hgs);
        var vgs = currentDialogueBox.GetComponentsInChildren<VerticalLayoutGroup>(true);
        cachedVLayouts.AddRange(vgs);
        var outs = currentDialogueBox.GetComponentsInChildren<Outline>(true);
        cachedOutlines.AddRange(outs);

        Canvas parentCanvas = currentDialogueBox.GetComponentInParent<Canvas>();
        if (parentCanvas != null) cachedRaycaster = parentCanvas.GetComponent<GraphicRaycaster>();
    }

    void ToggleLayoutAndRaycaster(bool enable)
    {
        if (currentDialogueBox == null) return;
        if (cachedFitters != null) foreach (var f in cachedFitters) if (f != null) f.enabled = enable;
        if (cachedHLayouts != null) foreach (var g in cachedHLayouts) if (g != null) g.enabled = enable;
        if (cachedVLayouts != null) foreach (var g in cachedVLayouts) if (g != null) g.enabled = enable;
        if (cachedOutlines != null) foreach (var o in cachedOutlines) if (o != null) o.enabled = enable;
        if (cachedRaycaster != null) cachedRaycaster.enabled = enable;
    }
    public void RestartDialogue()
    {
        currentDialogueIndex = 0;
        dialogues[0].message = "* I will wait outside for you, alright?";
        dialogues[1].message = "";
        CloseDialogue();
        ShowDialogue();
    }
    void ClampToCanvas(RectTransform target)
    {
        if (target == null) return;
        var canvas = target.GetComponentInParent<Canvas>();
        if (canvas == null) return;
        var root = (RectTransform)canvas.transform;
        Canvas.ForceUpdateCanvases();
        Vector2 boxSize = target.rect.size;
        Vector2 pos = target.anchoredPosition;
        Rect cr = root.rect;
        float halfW = boxSize.x * 0.5f;
        float halfH = boxSize.y * 0.5f;
        float minX = cr.xMin + halfW;
        float maxX = cr.xMax - halfW;
        float minY = cr.yMin + halfH;
        float maxY = cr.yMax - halfH;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        target.anchoredPosition = pos;
    }
}
