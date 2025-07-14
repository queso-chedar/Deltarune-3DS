using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistortionEffect : MonoBehaviour
{
    public Camera distortionCamera;
    public RawImage targetImage; // Asignar desde el Inspector
    private RenderTexture renderTexture;
    private Texture2D distortedTexture;
    
    void Start()
    {
        if (distortionCamera == null)
        {
            Debug.LogError("¡No hay cámara asignada!");
            return;
        }

        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        distortionCamera.targetTexture = renderTexture;
        distortedTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        
        if (targetImage == null)
            targetImage = GetComponent<RawImage>();
    }
    
    void Update()
    {
        StartCoroutine(ApplyDistortion());
    }
    
    IEnumerator ApplyDistortion()
    {
        if (renderTexture == null || distortedTexture == null)
            yield break;
        
        yield return new WaitForEndOfFrame();
        
        RenderTexture.active = renderTexture;
        distortedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        distortedTexture.Apply();
        
        Color[] pixels = distortedTexture.GetPixels();
        Color[] distortedPixels = new Color[pixels.Length];
        
        for (int y = 0; y < Screen.height; y++)
        {
            for (int x = 0; x < Screen.width; x++)
            {
                float wave = Mathf.Sin(x * 0.1f + Time.time * 5f) * 10f;
                int newX = x + (int)wave;
                newX = Mathf.Clamp(newX, 0, Screen.width - 1);
                distortedPixels[y * Screen.width + x] = pixels[y * Screen.width + newX];
            }
        }
        
        distortedTexture.SetPixels(distortedPixels);
        distortedTexture.Apply();
        
        if (targetImage != null)
            targetImage.texture = distortedTexture;
    }
}