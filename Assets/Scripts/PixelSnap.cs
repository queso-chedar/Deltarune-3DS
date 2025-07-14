using UnityEngine;

[ExecuteInEditMode]
public class PixelSnap : MonoBehaviour
{
    public float pixelsPerUnit = 40f;

    void LateUpdate()
    {
        float snap = 1f / pixelsPerUnit;
        Vector3 pos = transform.position;

        pos.x = Mathf.Round(pos.x / snap) * snap;
        pos.y = Mathf.Round(pos.y / snap) * snap;

        transform.position = pos;
    }
}
