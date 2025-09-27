using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class tor_cutsceneoutside : MonoBehaviour {

	public GameObject player;
	public torcartextcutscene torieltext;
	public GameObject fakeplayerobject;

	public GameObject torielobject;
	public Animator fakeplayeranimate;
	public Animator torielanimations;
	private float torielturncount;
	private bool textseen;
	private bool walkedleftplayer;
	private bool walkeddownplayer;
	private float torielkrisstarecount;
	private float currentstate;
	private float carwaittimer;
	private Animator animator;
	private float waitcartimer;
	void Start()
	{
		torielturncount = 3;
		textseen = false;
		walkedleftplayer = false;
		walkeddownplayer = false;
		torielkrisstarecount = 3;
		currentstate = -1;
		animator = GetComponent<Animator>();
		carwaittimer = 3;
		waitcartimer = 1.5f;
	}

	void Update()
	{
		if (currentstate == -1)
		{
			animator.Play("down", 0, 0f);
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
						fakeplayerobject.transform.position = Vector3.MoveTowards(fakeplayerobject.transform.position, new Vector3(-15.02f, fakeplayerobject.transform.position.y, fakeplayerobject.transform.position.z), Time.deltaTime * .8f);

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
								fakeplayerobject.transform.position = Vector3.MoveTowards(fakeplayerobject.transform.position, new Vector3(fakeplayerobject.transform.position.x, -3.85f, fakeplayerobject.transform.position.z), Time.deltaTime * .8f);
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

									if (Mathf.Approximately(fakeplayerobject.transform.position.x, -13.98f) && Mathf.Approximately(torieltext.transform.position.x, -13.98f))
									{
										waitcartimer -= 0.1f;
										if (waitcartimer <= 0)
										{
											Destroy(torielobject);
											Destroy(fakeplayerobject);
											currentstate = 0;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		else
		{
			if (currentstate == 0)
			{
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(-11.68f, transform.position.y, transform.position.z), Time.deltaTime * .8f);
				if (Mathf.Approximately(transform.position.x, -11.68f))
				{
					currentstate = 1;
				}

				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("left"))
				{
					animator.Play("left", 0, 0f);
				}
			}

			if (currentstate == 1)
			{
				carwaittimer -= 0.1f;
				if (carwaittimer <= 0)
				{
					currentstate = 2;
				}
			}

			if (currentstate == 2)
			{
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(-13.739f, transform.position.y, transform.position.z), Time.deltaTime * .8f);
				if (Mathf.Approximately(transform.position.x, -13.739f))
				{
					currentstate = 3;
				}
			}
			
			if (currentstate == 3)
			{
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -9.03f, transform.position.z), Time.deltaTime * 2f);
				if (Mathf.Approximately(transform.position.y, -9.03f))
				{
					currentstate = 4;
				}

				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("down"))
				{
					animator.Play("down", 0, 0f);
				}	
			}
		}
	}
}
