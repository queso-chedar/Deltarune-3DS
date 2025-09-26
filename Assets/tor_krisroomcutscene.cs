using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tor_krisroomcutscene : MonoBehaviour {
	public GameObject player;
	public DialogueSystem torieltext;
	public Animator bedsheet;
	public GameObject fakeplayerobject;
	public Animator fakeplayeranimate;
	public Animator torielanimations;
	private float torielwindowstandcount;
	private float krismovecountdown;
	private float playermovmentcountdown;
	private float torielmiddlestand;
	private bool seen;

	void Start()
	{
		torielwindowstandcount = 4;
		torielmiddlestand = 8;
		seen = false;
		krismovecountdown = 5;
		playermovmentcountdown = 10;
		player.SetActive(false);
	}

	void Update()
	{
		if (!Mathf.Approximately(torieltext.transform.position.y, -5))
		{
			if (torieltext.currentDialogueBox == null)
			{
				if (torielwindowstandcount > 0)
				{
					if (!torielanimations.GetCurrentAnimatorStateInfo(0).IsName("walkup"))
					{
						torielanimations.Play("walkup", 0, 0f);
					}
					torieltext.transform.position = Vector3.MoveTowards(torieltext.transform.position, new Vector3(torieltext.transform.position.x, 0.895f, torieltext.transform.position.z), Time.deltaTime * 1.5f);
					if (Mathf.Approximately(torieltext.transform.position.y, 0.895f))
					{
						torielanimations.Play("upstill", 0, 0f);
						torielwindowstandcount -= 0.1f;
						//Debug.Log("ssssansss");
					}
				}
				else
				{
					if (!torielanimations.GetCurrentAnimatorStateInfo(0).IsName("downwalk") && !torielanimations.GetCurrentAnimatorStateInfo(0).IsName("downstill") && !torielanimations.GetCurrentAnimatorStateInfo(0).IsName("right"))
					{
						if (torielanimations.GetCurrentAnimatorStateInfo(0).IsName("windowopen"))
						{
							if (torielanimations.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
								&& !torielanimations.IsInTransition(0))
							{
								torielanimations.Play("downwalk", 0, 0f);
							}
						}
						else
						{
							torielanimations.Play("windowopen", 0, 0f);
						}
					}
					else
					{
						if (!seen)
						{
							torieltext.transform.position = Vector3.MoveTowards(torieltext.transform.position, new Vector3(torieltext.transform.position.x, 0, torieltext.transform.position.z), Time.deltaTime * 1.5f);
						}

						if (seen)
						{
							torieltext.transform.position = Vector3.MoveTowards(torieltext.transform.position, new Vector3(torieltext.transform.position.x, -5, torieltext.transform.position.z), Time.deltaTime * 3f);
						}

						if (Mathf.Approximately(torieltext.transform.position.y, 0))
						{
							if (!torielanimations.GetCurrentAnimatorStateInfo(0).IsName("downstill") && !torielanimations.GetCurrentAnimatorStateInfo(0).IsName("right"))
								torielanimations.Play("downstill", 0, 0f);
							torielmiddlestand -= 0.1f;
						}
						if (torielanimations.GetCurrentAnimatorStateInfo(0).IsName("downstill"))
						{
							if (torielmiddlestand <= 4)
								torielanimations.Play("right", 0, 0f);
						}
						if (torielanimations.GetCurrentAnimatorStateInfo(0).IsName("right"))
						{
							if (torielmiddlestand <= 0 && !seen)
							{
								seen = true;
								torieltext.RestartDialogue();
							}
							if (seen && torieltext.currentDialogueBox == null)
							{
								torielanimations.Play("downwalk", 0, 0f);
							}
						}
					}
				}
			}
		}
		else
		{
			if (!bedsheet.GetCurrentAnimatorStateInfo(0).IsName("open"))
				bedsheet.Play("open", 0, 0f);
			else
			{
				krismovecountdown -= 0.1f;
				if (krismovecountdown <= 0)
				{
					fakeplayerobject.transform.position = Vector3.MoveTowards(fakeplayerobject.transform.position, new Vector3(1.33f, fakeplayerobject.transform.position.y, fakeplayerobject.transform.position.z), Time.deltaTime * 0.7f);

					if (!fakeplayeranimate.GetCurrentAnimatorStateInfo(0).IsName("move") && playermovmentcountdown == 10)
						fakeplayeranimate.Play("move", 0, 0f);
					
					if (Mathf.Approximately(fakeplayerobject.transform.position.x, 1.33f))
					{
						playermovmentcountdown -= 0.1f;
						fakeplayeranimate.Play("idle", 0, 0f);
						if (playermovmentcountdown <= 0)
						{
							Destroy(fakeplayerobject);
							Destroy(this.gameObject);
							player.SetActive(true);
						}
					}
				}
			}
		}
	}
}
