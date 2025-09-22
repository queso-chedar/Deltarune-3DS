using UnityEngine;
using UnityEngine.UI;

public class WaveScroll : MonoBehaviour
{
    public float speedX = 0.1f;  // velocidad de desplazamiento horizontal
    public float speedY = 0.05f; // velocidad vertical (puede usar 0 para solo horizontal)
    
    // Para RawImage (UI)
    private RawImage rawImage;
    private Rect uvRect;
    
    void Start() {
        // Si el componente es RawImage, obtenemos referencia
        rawImage = GetComponent<RawImage>();
        if (rawImage != null) {
            uvRect = rawImage.uvRect;
        }
    }

    void Update() {
        float dx = speedX * Time.deltaTime;
        float dy = speedY * Time.deltaTime;
        
        if (rawImage != null) {
            // Desplazamiento para RawImage
            uvRect.x += dx;
            uvRect.y += dy;
            // Mantener valores en rango [0,1] para loop perfecto
            uvRect.x %= 1f;
            uvRect.y %= 1f;
            rawImage.uvRect = uvRect;
        } else {
            // Si no es RawImage, intentamos con SpriteRenderer y material
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) {
                Material mat = sr.material;
                Vector2 offset = mat.mainTextureOffset;
                offset.x += dx;
                offset.y += dy;
                offset.x %= 1f;
                offset.y %= 1f;
                mat.mainTextureOffset = offset; 
            }
        }
    }
}
