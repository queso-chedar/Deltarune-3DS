using UnityEngine;
using System.Collections;

// NOTE
//
// Nintendo requires that games which allow player-to-player chat (etc.) must objain EULA agreement from the player beforehand.
//
// This example will behave differently, depending on whether or not this checkbox is checked:
//		Editor ->
//		PlayerSettings ->
//		Settings for Nintendo3DS ->
//		Publishing Settings ->
//		Show EULA
//
// If it's checked, the "AgreeEula" flag will be added to the 3DS's Banner Spec File.

public class EulaScript: MonoBehaviour
{
	void OnGUI()
	{
		if (UnityEngine.N3DS.Eula.IsAgreedTo())
		{
			GUI.Label(topRect, "EULA has been agreed to.");
		}
		else
		{
			// EULA agreement is a once-per-device setting (not once-per-game).
			// If you want to test this code path, you will need to clear the device's flag via the devkit's test menu .
			if (GUI.Button(topRect, "Show EULA"))
			{
				UnityEngine.N3DS.Eula.Show(true, true);
			}
		}
	}

	private Rect topRect = new Rect(60, 10, 200, 50);
}
