using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class changeroom_fade : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {	
		if (GetComponent<Image>().color.a >= 1)
		{
			SceneManager.LoadScene("fileselect_ch1");	
			Debug.Log("d");
		}
	}
}
