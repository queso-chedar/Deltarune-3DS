using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battlebox : MonoBehaviour {

	private Animator animator;
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("battlebox_create"))
		{
			Destroy(gameObject);
		}
	}
}
