using UnityEngine;
using System.Collections;
using System.Net;

public class TestNetSockets : MonoBehaviour
{
	void Start()
	{
		UnityEngine.N3DS.Account.Init(65536);
		stateHandler = DoShowStartButtons;
	}

	void OnGUI()
	{
		stateHandler.Invoke();
	}

	private void DoShowStartButtons()
	{
		Rect rect = new Rect(10, 10, 300, 40);
		if (GUI.Button(rect, "Start Server"))
		{
			TestTcpServer server = new TestTcpServer();
			server.Start(port);
			this.peer = server;
			stateHandler = DoShowState;
		}

		rect.y += 50;
		if (GUI.Button(rect, "Start Client"))
		{
			stateHandler = DoConnectClient;
		}
	}

	private void DoConnectClient()
	{
		if (GUI.Button(new Rect(10, 10, 300, 40), "Enter the server's IP Address"))
		{
			UnityEngine.N3DS.Keyboard.Show();
			stateHandler = DoWaitForKeyboardResult;
		}
	}

	private void DoWaitForKeyboardResult()
	{
		switch (UnityEngine.N3DS.Keyboard.GetResult())
		{
			case N3dsKeyboardResult.Invalid:
				// Skill processing.
				break;

			case N3dsKeyboardResult.Okay:
				{
					IPAddress ipAddr;
					if (IPAddress.TryParse(UnityEngine.N3DS.Keyboard.GetText(), out ipAddr))
					{
						TestTcpClient client = new TestTcpClient();
						client.Start(ipAddr, port);
						this.peer = client;
						stateHandler = DoShowState;
					}
					else
					{
						stateHandler = DoConnectClient;
					}
				}
				break;

			default:
				stateHandler = DoShowStartButtons;
				break;
		}
	}

	private void DoShowState()
	{
		peer.Display();
	}

	private delegate void StateHandler();
	private StateHandler stateHandler;
	private TestTcpPeer peer;
	private static readonly ushort port = 23456;
}
