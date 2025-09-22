using UnityEngine;
using UnityEngine.UI;

public class RainbowText : MonoBehaviour
{
    public float Color;
	public float Speed;

    void Update()
    {
        Color += Time.deltaTime * Speed;

        if (Color >= 0.9)
        {
            Color = 0f;
        }

        GetComponent<Text>().color = UnityEngine.Color.HSVToRGB(Color, 1, 1);
    }
}