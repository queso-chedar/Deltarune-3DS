using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skipintro : MonoBehaviour {
	public GameObject fadeout;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.A))
		{
			fadeout.SetActive(true);
		}
	}
}
