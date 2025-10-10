using UnityEngine;
using System.Collections;

public class SphereScript : MonoBehaviour
{
	void Update()
	{
		if (transform.position.y < -10.0f)
		{
			Destroy(gameObject);
		}
	}
}
