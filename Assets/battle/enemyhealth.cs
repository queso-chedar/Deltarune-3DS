using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyhealth : MonoBehaviour {

	public float health = 100;

	//string healthstring = health.ToString();

	Text changetext;
	public GameObject funnytext;
	void Start()
	{
		changetext = funnytext.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		changetext.text = "ENEMY HEALTH: " + health.ToString();
	}
}
