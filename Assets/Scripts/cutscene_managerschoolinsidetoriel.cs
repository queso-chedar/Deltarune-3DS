using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutscene_managerschoolinsidetoriel : MonoBehaviour {

	public float currentstate;
	private Animator torielanim;
	private float standholdcount;
	private float starecount;
	[SerializeField] private GameObject torielwalker;
	[SerializeField] private GameObject kriswalker;
	void Start()
	{
		currentstate = 1;
		torielanim = torielwalker.GetComponent<Animator>();
		standholdcount = 4;
		kriswalker.SetActive(false);
		starecount = 4;
	}

	// Update is called once per frame
	void Update()
	{
		if (currentstate == 1)
		{
			//make them exit thy car
			if (!torielanim.GetCurrentAnimatorStateInfo(0).IsName("torielupkris"))
				torielanim.Play("torielupkris", 0, 0f);

			//kriswalker.transform.position = Vector3.MoveTowards(kriswalker.transform.position, new Vector3(-0.36f, kriswalker.transform.position.y, kriswalker.transform.position.z), Time.deltaTime * .8f);
			torielwalker.transform.position = Vector3.MoveTowards(torielwalker.transform.position, new Vector3(torielwalker.transform.position.x, 2.2f, torielwalker.transform.position.z), Time.deltaTime * .8f);
			if (Mathf.Approximately(torielwalker.transform.position.y, 2.2f))
			{
				currentstate++;
				Debug.Log("to state 2");
			}
		}

		if (currentstate == 2)
		{
			standholdcount -= 0.1f;
			torielanim.Play("torielupkris", 0, 0f); //holds the frame
			if (standholdcount <= 0)
			{
				kriswalker.SetActive(true);
				currentstate++;
				Debug.Log("to state 3");
			}
		}

		if (currentstate == 3)
		{
			starecount -= 0.1f;
			torielanim.Play("torieldown", 0, 0f);
			if (starecount <= 0)
			{
				currentstate++;
				kriswalker.SetActive(false);
				Debug.Log("to state 4");
			}
		}

		if (currentstate == 4)
		{
			//toriel go up
			if (!torielanim.GetCurrentAnimatorStateInfo(0).IsName("torielhug"))
				torielanim.Play("torielhug", 0, 0f);

			if (torielanim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !torielanim.IsInTransition(0))
			{
				currentstate++;
				kriswalker.SetActive(true);
				Debug.Log("to state 5");
				starecount = 4;
			}
		}

		if (currentstate == 5)
		{
			starecount -= 0.1f;
			torielanim.Play("torieldown", 0, 0f);

			if (starecount <= 0)
			{
				currentstate++;
				Debug.Log("to state 6");
			}
		}

		if (currentstate == 6)
		{
			//toriel go up to school
			//if (!kriswalker.GetCurrentAnimatorStateInfo(0).IsName("torielupkris"))
				//kriswalker.Play("torielupkris", 0, 0f);

			kriswalker.transform.position = Vector3.MoveTowards(kriswalker.transform.position, new Vector3(-22.44f, kriswalker.transform.position.y, kriswalker.transform.position.z), Time.deltaTime * 1.6f);
			if (Mathf.Approximately(kriswalker.transform.position.x, -22.44f))
			{
				currentstate++;
				Debug.Log("to state 7");
			}
		}

		if (currentstate == 7)
		{
			//toriel go up to school
			//if (!kriswalker.GetCurrentAnimatorStateInfo(0).IsName("torielupkris"))
				//kriswalker.Play("torielupkris", 0, 0f);

			kriswalker.transform.position = Vector3.MoveTowards(kriswalker.transform.position, new Vector3(kriswalker.transform.position.x, 3.15f, kriswalker.transform.position.z), Time.deltaTime * .4f);
			if (Mathf.Approximately(kriswalker.transform.position.y, 3.15f))
			{
				currentstate++;
				Debug.Log("to state 8");
			}
		}
	}
}
