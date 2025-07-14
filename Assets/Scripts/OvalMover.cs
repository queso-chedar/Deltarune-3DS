using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class OvalMover : MonoBehaviour
{
    public EdgeCollider2D pathCollider;
    public float moveSpeed = 2f;
    public float offsetDistance = 0f; // Cuánto más atrás debe estar este objeto (en unidades del camino)

    private List<Vector2> worldPoints = new List<Vector2>();
    private float pathLength;
    private SpriteRenderer spriteRenderer;
    private float distanceTraveled = 0f;

    void Start()
    {
        if (pathCollider == null)
        {
            Debug.LogError("No EdgeCollider2D assigned!");
            enabled = false;
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Convertir puntos locales a mundo
        worldPoints.Clear();
        foreach (Vector2 point in pathCollider.points)
        {
            worldPoints.Add(pathCollider.transform.TransformPoint(point));
        }

        // Calcular longitud total del camino
        pathLength = 0f;
        for (int i = 0; i < worldPoints.Count - 1; i++)
        {
            pathLength += Vector2.Distance(worldPoints[i], worldPoints[i + 1]);
        }

        distanceTraveled = -offsetDistance; // aplicar offset inicial
    }

    void Update()
    {
        if (worldPoints.Count < 2) return;

        distanceTraveled += moveSpeed * Time.deltaTime;

        // Mantener distancia dentro del rango del path
        if (distanceTraveled > pathLength)
            distanceTraveled -= pathLength;

        Vector2 newPos = GetPositionAlongPath(distanceTraveled);
        Vector2 prevPos = transform.position;

        transform.position = newPos;

        Vector2 dir = (newPos - prevPos).normalized;
        if (dir.x != 0)
            spriteRenderer.flipX = dir.x < 0;
    }

    Vector2 GetPositionAlongPath(float distance)
    {
        float traveled = 0f;

        for (int i = 0; i < worldPoints.Count - 1; i++)
        {
            Vector2 start = worldPoints[i];
            Vector2 end = worldPoints[i + 1];
            float segmentLength = Vector2.Distance(start, end);

            if (traveled + segmentLength >= distance)
            {
                float t = (distance - traveled) / segmentLength;
                return Vector2.Lerp(start, end, t);
            }

            traveled += segmentLength;
        }

        // Si por algún motivo sobrepasamos, retornar el último punto
        return worldPoints[worldPoints.Count - 1];
    }
}