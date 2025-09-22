using UnityEngine;
using System.Collections;
using System.Text;

public class StreetPassExample : MonoBehaviour
{
	void Start()
	{
		state = TopMenu;
		streetPassId = UnityEngine.N3DS.StreetPass.Identifier;
	}

	void OnGUI()
	{
		state();

		GUI.Label(new Rect(5, 195, 310, 25), UnityEngine.N3DS.StreetPass.Status.ToString());
		GUI.Label(new Rect(5, 220, 310, 25), statusMessage);
	}

	void TopMenu()
	{
		if (GUI.Button(new Rect(5, 5, 310, 30), "Initialize StreetPass"))
		{
			N3dsStreetPassError result = UnityEngine.N3DS.StreetPass.Init(1024 * 1024);
			statusMessage = "StreetPass.Init() = " + result.ToString();

			state = MessageBoxMenu;
		}
	}

	void MessageBoxMenu()
	{
		Rect rect = new Rect(5, 5, 150, 26);
		if (GUI.Button(rect, "Create Box"))
		{
			CreateMessageBox();
		}

		rect.y += 30;
		if (GUI.Button(rect, "Destroy Box"))
		{
			DestroyMessageBox();
		}

		rect.y += 30;
		if (GUI.Button(rect, "Open Box"))
		{
			OpenMessageBox();
		}

		rect.y += 30;
		if (GUI.Button(rect, "Close Box"))
		{
			CloseMessageBox();
		}

		rect.y += 30;
		if (GUI.Button(rect, "Is Box Open"))
		{
			IsBoxOpen();
		}

		rect.y += 30;
		if (GUI.Button(rect, "Num Messages"))
		{
			GetNumMessages();
		}

		rect.x = 160;
		rect.y = 5;
		rect.width = 155;

		if (GUI.Button(rect, "Read Message"))
		{
			ReadMessage();
		}

		rect.y += 30;
		if (GUI.Button(rect, "Write Message"))
		{
			WriteMessage();
		}

		rect.y += 30;
		if (GUI.Button(rect, "Delete Messages"))
		{
			DeleteAllMessages();
		}

		rect.y += 30;
		if (GUI.Button(rect, "Restart Scan"))
		{
			RestartScanning();
		}
	}

	private bool CreateMessageBox()
	{
		N3dsStreetPassError err = UnityEngine.N3DS.StreetPass.MessageBox.Create
		(
			streetPassId,
			privateId,
			hmacKey,
			applicationName,
			"icon_48x48.tga",
			inboxSizeMax,
			outboxSizeMax,
			inboxMessNumMax,
			outboxMessNumMax
		);

		statusMessage = "CreateMessageBox: " + err.ToString();

		byte[] icon = UnityEngine.N3DS.StreetPass.LoadIcon("icon_48x48.tga");
		UnityEngine.N3DS.StreetPass.MessageBox.SetIcon(icon);

		return (err == N3dsStreetPassError.Success);
	}

	private bool DestroyMessageBox()
	{
		N3dsStreetPassError err = UnityEngine.N3DS.StreetPass.MessageBox.Delete(streetPassId);
		statusMessage = "DestroyMessageBox: " + err.ToString();
		return (err == N3dsStreetPassError.Success);
	}

	private bool OpenMessageBox()
	{
		N3dsStreetPassError err = UnityEngine.N3DS.StreetPass.MessageBox.Open(streetPassId, privateId);
		statusMessage = "OpenMessageBox: " + err.ToString();
		return (err == N3dsStreetPassError.Success);
	}

	private void CloseMessageBox()
	{
		UnityEngine.N3DS.StreetPass.MessageBox.Close();
		statusMessage = "CloseMessageBox: Okay";
	}

	private void IsBoxOpen()
	{
		bool isOpen = UnityEngine.N3DS.StreetPass.MessageBox.IsOpened();
		statusMessage = "Is Open: " + isOpen;
	}

	private void GetNumMessages()
	{
		uint numInbox = UnityEngine.N3DS.StreetPass.MessageBox.GetNumMessages(N3dsStreetPassBoxType.InBox);
		uint numOutbox = UnityEngine.N3DS.StreetPass.MessageBox.GetNumMessages(N3dsStreetPassBoxType.OutBox);
		statusMessage = "InBox: " + numInbox + ", OutBox: " + numOutbox;
	}

	private void ReadMessage()
	{
		uint numInbox = UnityEngine.N3DS.StreetPass.MessageBox.GetNumMessages(N3dsStreetPassBoxType.InBox);
		if (numInbox == 0)
		{
			statusMessage = "InBox is empty";
			return;
		}

		ulong messageId = UnityEngine.N3DS.StreetPass.MessageBox.GetMessageId(N3dsStreetPassBoxType.InBox, 0);
		UnityEngine.N3DS.StreetPass.Message message;
		N3dsStreetPassError err = UnityEngine.N3DS.StreetPass.MessageBox.ReadMessage(N3dsStreetPassBoxType.InBox, messageId, out message);
		statusMessage = "ReadMessage: " + err.ToString();

		Debug.Log("StreetPass Id:" + message.GetStreetPassId());
		Debug.Log("Message Id:" + message.GetMessageId());
		Debug.Log("Message Type:" + message.GetMessageType());
		Debug.Log("Header Size:" + message.GetHeaderSize());
		Debug.Log("Body Size:" + message.GetBodySize());
		Debug.Log("Total Size:" + message.GetTotalSize());

		System.DateTime createdDate = message.GetCreateDate();
		Debug.Log("Created: " + createdDate.ToLongDateString() + " " + createdDate.ToLongTimeString());

		System.DateTime sendDate = message.GetSendDate();
		Debug.Log("Sent: " + sendDate.ToLongDateString() + " " + sendDate.ToLongTimeString());

		System.DateTime receivedDate = message.GetRecvDate();
		Debug.Log("Received: " + receivedDate.ToLongDateString() + " " + receivedDate.ToLongTimeString());
		
		Debug.Log("GroupId:" + message.GetGroupId());
		Debug.Log("Hash:" + message.GetHashCode());
		Debug.Log("Send Mode:" + message.GetSendMode());
		Debug.Log("Send Count:" + message.GetSendCount());
		Debug.Log("Propagation Count:" + message.GetPropagationCount());

		byte[] body = message.GetBody();
		StringBuilder sb = new StringBuilder();
		int bytesToPrint = body.Length < 256 ? body.Length : 256;
		for (int index = 0; index < bytesToPrint; index++)
		{
			byte b = body[index];
			sb.Append(b.ToString("X2"));
			if ((index & 31) != 31)
			{
				sb.Append(',');
			}
			else
			{
				sb.AppendLine();
			}
		}

		Debug.Log("Body:");
		Debug.Log(sb.ToString());
	}

	private void WriteMessage()
	{
		UnityEngine.N3DS.StreetPass.Message message = new UnityEngine.N3DS.StreetPass.Message();
		N3dsStreetPassError err = message.Init
		(
			streetPassId,
			groupId,
			N3dsStreetPassMessageType.Anyone,
			N3dsStreetPassSendMode.SendRecv,
			UnityEngine.N3DS.StreetPass.Message.SENDCOUNT_UNLIMITED,
			1
		);
		if (err != N3dsStreetPassError.Success)
		{
			statusMessage = "message.Create: " + err.ToString();
			return;
		}

		byte[] body = new byte[1024];
		for (int index = 0; index < body.Length; index++)
		{
			body[index] = (byte)(index);
		}
		message.SetBody(body);

		byte[] icon = UnityEngine.N3DS.StreetPass.LoadIcon("icon_40x40.tga");
		err = message.SetIcon(icon);
		err = message.SetInfoText("An example of\ntwo-line InfoText.");

		ulong messageId;
		err = UnityEngine.N3DS.StreetPass.MessageBox.WriteMessage(ref message, N3dsStreetPassBoxType.OutBox, out messageId, true);

		statusMessage = "WriteMessage: " + err.ToString();
	}

	private void DeleteAllMessages()
	{
		N3dsStreetPassError err = UnityEngine.N3DS.StreetPass.MessageBox.DeleteAllMessages(N3dsStreetPassBoxType.InBox);
		if (err == N3dsStreetPassError.Success)
		{
			err = UnityEngine.N3DS.StreetPass.MessageBox.DeleteAllMessages(N3dsStreetPassBoxType.OutBox);
		}
		statusMessage = "Delete Messages: " + err;
	}

	private void RestartScanning()
	{
		N3dsStreetPassError err = UnityEngine.N3DS.StreetPass.StopScanning(true, false);
		statusMessage = "Stop: " + err.ToString();

		UnityEngine.N3DS.StreetPass.StartScanning(true);
		statusMessage += ",  Start: " + err.ToString();
	}

	private delegate void State();
	private State state;
	private string statusMessage = "";

	// 20-bit StreetPass ID.  Obtained from Nintendo.
	// This is set via the editor's Player Settings, since it needs fed into the ROM image build process.
	private uint streetPassId;

	// Message grouping ID.  If set to a value other than zero, messages with the same group ID are sent together.
	private readonly uint groupId = 0;

	// 32-bit secret key, required to open the message box.
	private readonly uint privateId = 0x89ABCDEF;

	// 32-byte secret key, required for reading messages.
	private readonly byte[] hmacKey = new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

	private readonly string applicationName = "StreetPassExample";

	private readonly int inboxSizeMax = 100 * 1024;
	private readonly int outboxSizeMax = 100 * 1024;
	private readonly int inboxMessNumMax = 50;
	private readonly int outboxMessNumMax = 50;
}
