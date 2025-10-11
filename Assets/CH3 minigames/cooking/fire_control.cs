using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_control : MonoBehaviour {

	private float firecreatecountdown;
	private float firewarncountdown;
	public GameObject fire;
	public GameObject fire_warn;

	void Start () {
		firecreatecountdown = 180;
		firewarncountdown = 140;
		Instantiate(fire, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		firecreatecountdown -= 1;
		firewarncountdown -= 1;

		if (firecreatecountdown <= 0)
		{
			firecreatecountdown = 180;
			Instantiate(fire, transform.position, transform.rotation);
		}
		
		if (firewarncountdown <= 0)
        {
			firewarncountdown = 180;
			Instantiate(fire_warn, new Vector3(-3.158f, 0.746f, 0f), transform.rotation);
        }
	}
}
