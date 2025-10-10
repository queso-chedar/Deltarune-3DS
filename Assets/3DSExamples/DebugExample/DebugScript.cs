using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour
{
	void Update()
	{
		Vector3 spawnPos = new Vector3(Random.Range(-2.0f, +2.0f), 8.0f, Random.Range(-2.0f, +2.0f));
		Instantiate(spawnObject, spawnPos, transform.rotation);
	}

	void OnGUI()
	{
		Rect rect = new Rect(10, 10, 200, 30);
		GUI.Label(rect, "   Sys : " + UnityEngine.N3DS.Debug.GetSystemFree());

		rect.y += 40;
		GUI.Label(rect, "Device : " + UnityEngine.N3DS.Debug.GetDeviceFree());

		rect.y += 40;
		GUI.Label(rect, "VRAM A : " + UnityEngine.N3DS.Debug.GetVRAMAFree());

		rect.y += 40;
		GUI.Label(rect, "VRAM B : " + UnityEngine.N3DS.Debug.GetVRAMBFree());

		rect.y += 40;
		if (GUI.Button(rect, "Crash"))
		{
			UnityEngine.N3DS.Debug.Crash("Boom!");
		}
	}

	public Transform spawnObject;
}
