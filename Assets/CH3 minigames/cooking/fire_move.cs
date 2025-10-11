using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(5.84f, transform.position.y, transform.position.z), Time.deltaTime * 1.6f);

		if (Mathf.Approximately(transform.position.x, 5.84f) && gameObject.name.Contains("(Clone)"))
        {
			Destroy(gameObject);
        }
	}
}
