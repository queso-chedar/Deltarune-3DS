    using UnityEngine;

    public class CameraFollow : MonoBehaviour
    {
        public Transform target; // The player object the camera will follow
        public Vector3 offset;   // The desired distance and direction from the player
        public float smoothSpeed = 0.125f; // Controls the smoothness of the camera movement

        void Update()
        {
            if (target != null) // Make sure a target is assigned
            {
                // Calculate the desired position for the camera
                Vector3 desiredPosition = target.position + offset;

                // Smoothly interpolate the camera's position to the desired position
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

                // Set the camera's new position
                transform.position = smoothedPosition;
            }
        }
    }