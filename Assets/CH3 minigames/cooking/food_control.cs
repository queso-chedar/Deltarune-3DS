using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food_control : MonoBehaviour {

	private float foodcreatecountdown;
	private float foodwarncountdown;
	public GameObject food;
	public GameObject food_warn;
		
	void Start () {
		foodcreatecountdown = 60;
		foodwarncountdown = 40;
		Instantiate(food, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		foodcreatecountdown -= 1;
		foodwarncountdown -= 1;

		if (foodcreatecountdown <= 0)
		{
			foodcreatecountdown = 60;
			Instantiate(food, transform.position, transform.rotation);
		}
		
		if (foodwarncountdown <= 0)
        {
			foodwarncountdown = 40;
			Instantiate(food_warn, new Vector3(3.103f, 0.533f, 0f), transform.rotation);
        }
	}
}
