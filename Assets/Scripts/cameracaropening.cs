using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cameracaropening : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update()
	{
		if (SceneManager.GetActiveScene().name == "room_town_school")
		{
			Vector3 target = new Vector3(0.3188291f, transform.position.y, transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);

			if (transform.position == target)
			{
				enabled = false;
			}
		}
		else
		{
			Vector3 target = new Vector3(transform.position.x+0.1f, transform.position.y, transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
		}
	}
}
