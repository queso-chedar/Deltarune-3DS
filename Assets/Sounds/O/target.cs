using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
	public AudioSource audio;
	public float RepeatTime = 0.1f;
	public int MaxRepeats = 5;
	[Header("Debug Info")]
	public int CurrentRepeats = 0;


	// Use this for initialization
	void Start()
	{
		StartCoroutine("Target");
	}

	// Update is called once per frame
	void Update()
	{
      		if (CurrentRepeats >= MaxRepeats)
		{
			StopCoroutine("Target");
        }
	}
	IEnumerator Target()
	{
		yield return new WaitForSeconds(RepeatTime);
		audio = GetComponent<AudioSource>();
		audio.PlayOneShot(audio.clip);
		StartCoroutine("Target");
		CurrentRepeats = CurrentRepeats + 1;
	}
}
