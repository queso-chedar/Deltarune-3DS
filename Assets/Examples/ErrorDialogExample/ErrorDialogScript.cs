using UnityEngine;
using System.Collections;

public class ErrorDialogScript: MonoBehaviour
{
	void Start()
	{
		ChangeEnumType(+1);
		UnityEngine.N3DS.Account.Init(65536);
	}

	void OnGUI()
	{
		int ypos = 10;

		if (GUI.Button(new Rect(10, ypos, 145, 40), "allowHome = " + allowHome))
		{
			allowHome = !allowHome;
		}

		if (GUI.Button(new Rect(165, ypos, 145, 40), "allowReset = " + allowReset))
		{
			allowReset = !allowReset;
		}

		if (GUI.Button(new Rect(10, ypos += 50, 40, 40), "<"))
		{
			ChangeEnumType(-1);
		}

		GUI.Label(new Rect(60, ypos, 200, 40), "Type:" + enumTypes[enumType]);

		if (GUI.Button(new Rect(270, ypos, 40, 40), ">"))
		{
			ChangeEnumType(+1);
		}

		if (GUI.Button(new Rect(10, ypos += 50, 40, 40), "<"))
		{
			if (--enumValue == 0)
			{
				enumValue = enumValues.Length - 1;
			}
		}

		GUI.Label(new Rect(60, ypos, 200, 40), "Val:" + enumValues.GetValue(enumValue).ToString());

		if (GUI.Button(new Rect(270, ypos, 40, 40), ">"))
		{
			if (++enumValue >= enumValues.Length)
			{
				enumValue = 0;
			}
		}

		if (GUI.Button(new Rect(10, ypos += 50, 93, 40), "System Error"))
		{
			switch (enumType)
			{
				case 0:
					UnityEngine.N3DS.ErrorViewer.Show((N3dsAccountError)enumValue, allowHome, allowReset);
					break;
				case 1:
					// Access point library does not have the ability to convert results into error codes.
					// We can only show the last error.
					UnityEngine.N3DS.AccessPoint.TryShowLastError(allowHome, allowReset);
					break;
			}
		}

		if (GUI.Button(new Rect(113, ypos, 93, 40), "User Error"))
		{
			UnityEngine.N3DS.ErrorViewer.Show("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", true, allowHome, allowReset);
		}

		if (GUI.Button(new Rect(216, ypos, 93, 40), "Numeric Error"))
		{
            UnityEngine.N3DS.ErrorViewer.Show((N3dsAccountError)42, allowHome, allowReset);
		}

		GUI.Label(new Rect(20, ypos += 50, 300, 40), "Result: " + UnityEngine.N3DS.ErrorViewer.Result);
	}

	private void ChangeEnumType(int direction)
	{
		enumType += direction;
		if (enumType < 0)
		{
			enumType = enumTypes.Length - 1;
		}
		else if (enumType >= enumTypes.Length)
		{
			enumType = 0;
		}

		switch (enumType)
		{
			case 0:
				{
					string clientId = "wibble";
					int usableRangeInSeconds = 60;
					string token, iv, signature, environment;
					UnityEngine.N3DS.Account.AcquireIndependentServiceToken(clientId, usableRangeInSeconds, out token, out iv, out signature, out environment);
                    enumValues = System.Enum.GetValues(typeof(N3dsAccountError));
				}
				break;

			case 1:
				enumValues = accessPointTypes;
				break;
		}

		enumValue = 0;
	}

	private readonly string[] enumTypes = { "Account", "AccessPoint" };
	private readonly string[] accessPointTypes = { "Last Error" };

	private bool allowHome = false;
	private bool allowReset = false;
	private int enumType = 0;
	private int enumValue = 0;
	private System.Array enumValues;
}
