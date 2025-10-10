using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_openingschool : MonoBehaviour {

	private float currentstate;
	private Animator animator;
	public NewKrisController krisController;
	public cutscene_managerschooloutside cutscene_manager;
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
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1.14f, transform.position.y, transform.position.z), Time.deltaTime * 2f);
			if (Mathf.Approximately(transform.position.x, -1.14f))
			{
				currentstate ++;
			}

			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("right"))
			{
				animator.Play("right", 0, 0f);
			}
		}

		if (currentstate == 1)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 4.92f, transform.position.z), Time.deltaTime * .8f);
			if (Mathf.Approximately(transform.position.y, 4.92f))
			{
				currentstate++;
				cutscene_manager.currentstate = 0;
			}

			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("up"))
			{
				animator.Play("up", 0, 0f);
			}
		}	
	}
}
