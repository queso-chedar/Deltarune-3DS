using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tor_cutsceneoutside : MonoBehaviour {

	public GameObject player;
	public torcartextcutscene torieltext;
	public GameObject fakeplayerobject;
	public Animator fakeplayeranimate;
	public Animator torielanimations;
	private float torielturncount;
	private bool textseen;
	private bool walkedleftplayer;
	private bool walkeddownplayer;
	private float torielkrisstarecount;
	void Start()
	{
		torielturncount = 3;
		textseen = false;
		walkedleftplayer = false;
		walkeddownplayer = false;
		torielkrisstarecount = 3;
	}

	void Update()
	{
		torielturncount -= 0.1f;
		if (torielturncount <= 0)
		{
			if (!torielanimations.GetCurrentAnimatorStateInfo(0).IsName("upstill") && !textseen)
			{
				torielanimations.Play("upstill", 0, 0f);
				//torieltext.krisController.enabled = false;
				torieltext.ShowDialogue();
				textseen = true;
			}
			if (torieltext.currentDialogueBox == null && textseen && !walkedleftplayer)
			{
				fakeplayeranimate.Play("walkleft", 0, 0f);
				walkedleftplayer = true;
			}
			if (walkedleftplayer)
			{
				if (!fakeplayeranimate.GetCurrentAnimatorStateInfo(0).IsName("walkright"))
					fakeplayerobject.transform.position = Vector3.MoveTowards(fakeplayerobject.transform.position, new Vector3(-15.02f, fakeplayerobject.transform.position.y, fakeplayerobject.transform.position.z), Time.deltaTime * 1.5f);

				if (Mathf.Approximately(fakeplayerobject.transform.position.x, -15.02f) || fakeplayeranimate.GetCurrentAnimatorStateInfo(0).IsName("walkright"))
				{
					if (!walkeddownplayer)
					{
						fakeplayeranimate.Play("walkdown", 0, 0f);
						walkeddownplayer = true;
					}
					else
					{

						if (!Mathf.Approximately(fakeplayerobject.transform.position.y, -3.85f))
						{
							fakeplayerobject.transform.position = Vector3.MoveTowards(fakeplayerobject.transform.position, new Vector3(fakeplayerobject.transform.position.x, -3.85f, fakeplayerobject.transform.position.z), Time.deltaTime * 1.5f);
						}
						else
						{
							if (torielkrisstarecount > 0)
							{
								torielkrisstarecount -= 0.1f;
								fakeplayeranimate.Play("walkright", 0, 0f);
								torielanimations.Play("leftstill", 0, 0f);
							}
							if (torielkrisstarecount <= 0)
							{
								fakeplayerobject.transform.position = Vector3.MoveTowards(fakeplayerobject.transform.position, new Vector3(-13.98f, fakeplayerobject.transform.position.y, fakeplayerobject.transform.position.z), Time.deltaTime * 1f);
								torieltext.transform.position = Vector3.MoveTowards(torieltext.transform.position, new Vector3(-13.98f, torieltext.transform.position.y, torieltext.transform.position.z), Time.deltaTime * 1f);
							}
						}
					}
				}
			}
		}
	}
}
