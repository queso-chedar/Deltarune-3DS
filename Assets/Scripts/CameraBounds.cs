using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Transform target;  // El objeto que la cámara sigue (por ejemplo, el jugador)
    public float minX, maxX, minY, maxY;  // Límites de la cámara en X y Y

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        // Obtener la posición de la cámara
        Vector3 cameraPos = transform.position;

        // Mantener la cámara dentro de los límites definidos
        cameraPos.x = Mathf.Clamp(target.position.x, minX + cam.orthographicSize * cam.aspect, maxX - cam.orthographicSize * cam.aspect);
        cameraPos.y = Mathf.Clamp(target.position.y, minY + cam.orthographicSize, maxY - cam.orthographicSize);

        // Aplicar la nueva posición a la cámara
        transform.position = cameraPos;
    }
}