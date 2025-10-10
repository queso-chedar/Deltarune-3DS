using UnityEngine;
using System.Collections;

public class Tumble : MonoBehaviour
{
	void Start()
	{
		yawD = Random.Range(-100.0f, +100.0f);
		pitchD = Random.Range(-100.0f, +100.0f);
		rollD = Random.Range(-100.0f, +100.0f);
	}

	void Update()
	{
		float deltaTime = Time.deltaTime;
		yaw += yawD * deltaTime;
		pitch += pitchD * deltaTime;
		roll += rollD * deltaTime;
		transform.rotation = Quaternion.Euler(yaw, pitch, roll);
	}

	private float yaw, pitch, roll;
	private float yawD, pitchD, rollD;
}
