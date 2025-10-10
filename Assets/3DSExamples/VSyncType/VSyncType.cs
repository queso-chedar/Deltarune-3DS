using UnityEngine;
using System.Collections;

public class VSyncType : MonoBehaviour
{
	// ---- Unity Hooks ----

	void Start()
	{
		// There's no API to determine the Editor setting for "VSync Type", it's an editor-only setting.
		// However, for the purposes of this example, we can work it out.
		bool wasTripleBuffered = UnityEngine.N3DS.Screen.IsTripleBuffered();
		UnityEngine.N3DS.Screen.SetIsTripleBuffered(!wasTripleBuffered);
		bool isTripleBuffered = UnityEngine.N3DS.Screen.IsTripleBuffered();
		if (isTripleBuffered != wasTripleBuffered)
		{
			type = Type.kRuntime;
			UnityEngine.N3DS.Screen.SetIsTripleBuffered(wasTripleBuffered);
		}
		else
		{
			type = isTripleBuffered ? Type.kTriple : Type.kDouble;
		}

		for (int x = -4; x <= 4; x++)
		{
			for (int y = -4; y <= 4; y++)
			{
				for (int z = -3; z <= 3; z++)
				{
					Instantiate(cubePrefab, new Vector3(x * 2, y * 2, z * 2), Quaternion.identity);
				}
			}
		}
	}

	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		bool isTripleBuffered = UnityEngine.N3DS.Screen.IsTripleBuffered();

		Rect rect = new Rect(60, 10, 200, 50);
		GUI.Label(rect, "VSync Type : " + typeNames[(int)type]);

		rect.y += 40;
		GUI.Label(rect, "IsTripleBuffered() : " + isTripleBuffered);

		rect.y += 40;
		GUI.Label(rect, string.Format("fps : {0:0.}", 1.0f / deltaTime));

		rect.y += 40;
		if (type == Type.kRuntime)
		{
			if (GUI.Button(rect, "Switch"))
			{
				UnityEngine.N3DS.Screen.SetIsTripleBuffered(!isTripleBuffered);
			}
		}
	}

	// ---- Variables ----

	private enum Type { kDouble, kTriple, kRuntime };
	private Type type;

	private static readonly string[] typeNames = { "Double Buffered", "Triple Buffered", "Runtime" };

	private float deltaTime = 0.0f;

	public Transform cubePrefab;
}
