using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RainbowText_V1 : MonoBehaviour
{
    [SerializeField] private Gradient textGradient;
    [SerializeField] private float gradientSpeed = 1f;
    [SerializeField] private float updateRate = 0.05f; // Actualiza cada 50ms (20 FPS)

    private Text uiText;
    private float totalTime;
    private string baseText;

    void Awake()
    {
        uiText = GetComponent<Text>();
        baseText = uiText.text; // Guarda el texto original
    }

    void Start()
    {
        StartCoroutine(AnimateRainbow());
    }

    IEnumerator AnimateRainbow()
    {
        while (true)
        {
            string coloredText = "";
            int charCount = baseText.Length;

            for (int i = 0; i < charCount; i++)
            {
                float offset = (float)i / charCount;
                Color color = textGradient.Evaluate((totalTime + offset) % 1f);
                string hexColor = ColorUtility.ToHtmlStringRGB(color);
                coloredText += string.Format("<color=#{0}>{1}</color>", hexColor, baseText[i]);
            }

            uiText.text = coloredText;

            totalTime += gradientSpeed * updateRate;

            yield return new WaitForSeconds(updateRate);
        }
    }
}