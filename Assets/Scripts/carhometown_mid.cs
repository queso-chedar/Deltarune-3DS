using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carhometown_mid : MonoBehaviour {

	private float currentstate;
	private Animator animator;
	public NewKrisController krisController;

	void Start()
	{
		currentstate = 0;
		animator = GetComponent<Animator>();
		animator.Play("down", 0, 0f);
		krisController.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (currentstate == 0)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -0.71f, transform.position.z), Time.deltaTime * 2f);
			if (Mathf.Approximately(transform.position.y, -0.71f))
			{
				currentstate ++;
			}

			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("down"))
			{
				animator.Play("down", 0, 0f);
			}
		}

		if (currentstate == 1)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(16.5f, transform.position.y, transform.position.z), Time.deltaTime * 2f);
			if (Mathf.Approximately(transform.position.x, 16.5f))
			{
				currentstate ++;
			}

			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("right"))
			{
				animator.Play("right", 0, 0f);
			}
		}	
	}
}
