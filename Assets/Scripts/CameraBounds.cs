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
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth  = camHalfHeight * cam.aspect;

        if (maxX - minX > camHalfWidth * 2) 
            cameraPos.x = Mathf.Clamp(target.position.x, minX + camHalfWidth, maxX - camHalfWidth);
        if (maxY - minY > camHalfHeight * 2) 
            cameraPos.y = Mathf.Clamp(target.position.y, minY + camHalfHeight, maxY - camHalfHeight);



        // Aplicar la nueva posición a la cámara
        transform.position = cameraPos;
    }
}