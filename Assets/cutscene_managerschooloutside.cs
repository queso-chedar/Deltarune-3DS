using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutscene_managerschooloutside : MonoBehaviour {

	private float exitcarcountdown;
	public float currentstate;
	private Animator torielanim;
	
	[SerializeField] private GameObject torielwalker;
	[SerializeField] private GameObject kriswalker;
	void Start()
	{
		currentstate = -1;
		exitcarcountdown = 5f;
		torielanim = torielwalker.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (currentstate == 0)
		{
			exitcarcountdown -= 0.1f;
			if (exitcarcountdown <= 0)
			{
				//make toriel and kris go to the car x and y
				kriswalker.transform.position = new Vector3(-1.21f, 4.63f, kriswalker.transform.position.z);
				torielwalker.transform.position = new Vector3(-1.19f, 5.01f, torielwalker.transform.position.z);
				currentstate++;
				Debug.Log("to state 1");
			}
		}

		if (currentstate == 1)
		{
			//make them exit thy car
			kriswalker.transform.position = Vector3.MoveTowards(kriswalker.transform.position, new Vector3(-0.36f, kriswalker.transform.position.y, kriswalker.transform.position.z), Time.deltaTime * .8f);
			torielwalker.transform.position = Vector3.MoveTowards(torielwalker.transform.position, new Vector3(-2.34f, torielwalker.transform.position.y, torielwalker.transform.position.z), Time.deltaTime * .8f);
			if (Mathf.Approximately(torielwalker.transform.position.x, -2.34f) && Mathf.Approximately(kriswalker.transform.position.x, -0.36f))
			{
				currentstate++;
				Debug.Log("to state 2");
			}
		}

		if (currentstate == 2)
		{
			//toriel go down
			if (!torielanim.GetCurrentAnimatorStateInfo(0).IsName("torieldown"))
				torielanim.Play("torieldown", 0, 0f);
			
			torielwalker.transform.position = Vector3.MoveTowards(torielwalker.transform.position, new Vector3(torielwalker.transform.position.x, 4.12f, torielwalker.transform.position.z), Time.deltaTime * .8f);
			if (Mathf.Approximately(torielwalker.transform.position.y, 4.12f))
			{
				currentstate++;
				Debug.Log("to state 3");
			}
		}

		if (currentstate == 3)
		{
			//toriel go right
			if (!torielanim.GetCurrentAnimatorStateInfo(0).IsName("torielright"))
				torielanim.Play("torielright", 0, 0f);

			torielwalker.transform.position = Vector3.MoveTowards(torielwalker.transform.position, new Vector3(0.47f, torielwalker.transform.position.y, torielwalker.transform.position.z), Time.deltaTime * .8f);
			if (Mathf.Approximately(torielwalker.transform.position.x, 0.47f))
			{
				currentstate++;
				Debug.Log("to state 4");
			}
		}

		if (currentstate == 4)
		{
			//toriel go up
			if (!torielanim.GetCurrentAnimatorStateInfo(0).IsName("torielup"))
				torielanim.Play("torielup", 0, 0f);

			torielwalker.transform.position = Vector3.MoveTowards(torielwalker.transform.position, new Vector3(torielwalker.transform.position.x, 5.25f, torielwalker.transform.position.z), Time.deltaTime * .8f);
			if (Mathf.Approximately(torielwalker.transform.position.y, 5.25f))
			{
				currentstate++;
				kriswalker.SetActive(false);
				Debug.Log("to state 5");
			}
		}

		if (currentstate == 5)
		{
			//toriel go right to school
			if (!torielanim.GetCurrentAnimatorStateInfo(0).IsName("torielrightkris"))
				torielanim.Play("torielrightkris", 0, 0f);

			torielwalker.transform.position = Vector3.MoveTowards(torielwalker.transform.position, new Vector3(1.30f, torielwalker.transform.position.y, torielwalker.transform.position.z), Time.deltaTime * .8f);
			if (Mathf.Approximately(torielwalker.transform.position.x, 1.30f))
			{
				currentstate++;
				Debug.Log("to state 6");
			}
		}

		if (currentstate == 6)
		{
			//toriel go up to school
			if (!torielanim.GetCurrentAnimatorStateInfo(0).IsName("torielupkris"))
				torielanim.Play("torielupkris", 0, 0f);

			torielwalker.transform.position = Vector3.MoveTowards(torielwalker.transform.position, new Vector3(torielwalker.transform.position.x, 10f, torielwalker.transform.position.z), Time.deltaTime * .8f);
			if (Mathf.Approximately(torielwalker.transform.position.y, 10f))
			{
				currentstate++;
				kriswalker.SetActive(false);
				Debug.Log("to state 7");
			}
		}
	}
}
