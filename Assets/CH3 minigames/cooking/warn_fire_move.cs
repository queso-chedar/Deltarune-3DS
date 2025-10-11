using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warn_fire_move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(-4.671f, 3.839f, transform.position.z), Time.deltaTime * 4f);

		if (Mathf.Approximately(transform.position.x, -4.671f) && Mathf.Approximately(transform.position.y, 3.839f) && gameObject.name.Contains("(Clone)"))
        {
			
			Destroy(gameObject);
        }
	}
}
