using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugName : MonoBehaviour
{
public Text logText; // Text component that will display the logs
[SerializeField]
private Text deviceText;
[SerializeField]
private Text systemText;
private Queue<string> logMessages = new Queue<string>(); // Queue that stores the log messages
private const int maxLines = 4; // Max amount of lines (logs) that will be shown

void OnEnable()
{
// Register the HandleLog callback
Application.logMessageReceived += HandleLog;
}

void OnDisable()
{
// Unregister the HandleLog callback
Application.logMessageReceived -= HandleLog;
}

void HandleLog(string logString, string stackTrace, LogType type)
{
logMessages.Enqueue(logString);

// Remove the oldest message if the queue exceeds the maximum line count
if (logMessages.Count > maxLines)
{
logMessages.Dequeue();
}

// Update the UI Text component
if (logText != null)
{
logText.text = string.Join("\n", logMessages.ToArray());
}
}

void Update()
    {
int freeMem = UnityEngine.N3DS.Debug.GetDeviceFree();
int freeSys = UnityEngine.N3DS.Debug.GetSystemFree();

deviceText.text = "Available D RAM: " + (freeMem);
systemText.text = "Available S RAM: " + (freeSys);
}
}