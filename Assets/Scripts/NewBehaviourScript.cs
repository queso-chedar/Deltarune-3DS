using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
public AudioSource Sound1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		IEnumerator fadeSound1 = AudioFadeOut.FadeOut (Sound1, 0.5f);
StartCoroutine (fadeSound1);
StopCoroutine (fadeSound1);
	}
}
