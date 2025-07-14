using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [Header("Configuración")]
    public float updateInterval = 0.5f;
    public Color textColor = Color.green;
    public int fontSize = 20;
    public Vector2 screenPosition = new Vector2(0.02f, 0.96f);
    public bool useDynamicColor = true;
    
    [Header("Umbrales Color Dinámico")]
    public float highThreshold = 50f;
    public float mediumThreshold = 30f;

    private float _accumulatedFrames;
    private int _frameCount;
    private float _timeLeft;
    private float _currentFPS;
    private Text _fpsText;
    private GameObject _textObject;

    void Start()
    {
        CreateFPSText();
        _timeLeft = updateInterval;
    }

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        _accumulatedFrames += Time.timeScale / Time.deltaTime;
        _frameCount++;

        if (_timeLeft <= 0f && _fpsText != null)
        {
            _currentFPS = _accumulatedFrames / _frameCount;
            _timeLeft = updateInterval;
            _accumulatedFrames = 0f;
            _frameCount = 0;

            // Versión compatible con C# 4.0
            _fpsText.text = "FPS: " + Mathf.RoundToInt(_currentFPS).ToString();
            
            if (useDynamicColor)
            {
                _fpsText.color = GetFPSColor(_currentFPS);
            }
            else
            {
                _fpsText.color = textColor;
            }
        }
    }

    private void CreateFPSText()
    {
        if (_textObject != null) return;

        _textObject = new GameObject("FPS Display");
        
        Canvas canvas = _textObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;

        _fpsText = _textObject.AddComponent<Text>();
        _fpsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        _fpsText.fontSize = fontSize;
        _fpsText.color = textColor;
        _fpsText.alignment = TextAnchor.UpperLeft;

        RectTransform rectTransform = _textObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = screenPosition;
        rectTransform.anchorMax = screenPosition;
        rectTransform.pivot = screenPosition;
        rectTransform.anchoredPosition = Vector2.zero;
        
        DontDestroyOnLoad(_textObject);
    }

    private Color GetFPSColor(float fps)
    {
        if (fps >= highThreshold) return Color.green;
        if (fps >= mediumThreshold) return Color.yellow;
        return Color.red;
    }

    void OnDestroy()
    {
        if (_textObject != null)
        {
            Destroy(_textObject);
        }
    }
}