using UnityEngine;

public class moveleftdumbass : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}