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

	void Start () {
		torielturncount = 3;
	}

	void Update()
	{
		torielturncount -= 0.1f;
		if (torielturncount <= 0)
		{
			if (!torielanimations.GetCurrentAnimatorStateInfo(0).IsName("upstill"))
			{
				torielanimations.Play("upstill", 0, 0f);
				//torieltext.krisController.enabled = false;
				torieltext.ShowDialogue();
			}
		}
	}
}
