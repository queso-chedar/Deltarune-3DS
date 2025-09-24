using UnityEngine;

public class rudinshoot : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}