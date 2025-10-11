using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadow_guy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(-5.68f, transform.position.y, transform.position.z), Time.deltaTime * .6f);
	}
}
