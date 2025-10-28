using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool followY = true;
    public bool followX = true;

    void Update()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            desiredPosition.z = transform.position.z;

            if (!followY)
                desiredPosition.y = transform.position.y;

            if (!followX)
                desiredPosition.x = transform.position.x;

            transform.position = desiredPosition;
        }
    }
}
