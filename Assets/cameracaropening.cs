using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracaropening : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update()
	{
        Vector3 target = new Vector3(0.3188291f, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime);

        if (transform.position == target)
		{
			enabled = false;
		}
	}
}
