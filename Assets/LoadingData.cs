using UnityEngine;
public class LoadingData : MonoBehaviour
{
	public AudioClip[] AudioClipsToPreload;
	public AudioClip[] AudioClipsToUnload;
	void Start()
	{
		foreach (AudioClip Clips in AudioClipsToPreload)
		{
			if (Clips != null)
			{
				Clips.LoadAudioData();
			}
		}
		foreach (AudioClip Clips in AudioClipsToUnload)
		{
			if (Clips != null)
			{
				Clips.UnloadAudioData();
			}
		}
	}
}