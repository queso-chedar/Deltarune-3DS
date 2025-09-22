using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mutesong : MonoBehaviour {
	public AudioSource audioSource;
	void Start () {
		audioSource.mute = true;
	}
}
