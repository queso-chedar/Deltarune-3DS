using UnityEngine;

public class HomeButtonScript : MonoBehaviour
{
	void OnGUI()
	{
		bool isEnabled = UnityEngine.N3DS.HomeButton.IsEnabled();

		cycle += Time.deltaTime;
		float ypos = 88.0f + Mathf.Sin(cycle) * 10.0f;

		if (GUI.Button(new Rect(80, ypos, 160, 40), isEnabled ? " Disable Home Button " : " Enable Home Button"))
		{
			if (isEnabled)
			{
				UnityEngine.N3DS.HomeButton.Disable();
			}
			else
			{
				UnityEngine.N3DS.HomeButton.Enable();
			}
		}
	}

	private float cycle = 0.0f;
}
