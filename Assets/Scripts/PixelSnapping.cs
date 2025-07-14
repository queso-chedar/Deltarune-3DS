using UnityEngine;

[ExecuteInEditMode]
public class PixelSnapping : MonoBehaviour
{
    public float pixelsPerUnit = 40f;

    void LateUpdate()
    {
        float snapValue = 1f / pixelsPerUnit;
        Vector3 pos = transform.position;

        pos.x = Mathf.Round(pos.x / snapValue) * snapValue;
        pos.y = Mathf.Round(pos.y / snapValue) * snapValue;

        transform.position = pos;
    }
}
