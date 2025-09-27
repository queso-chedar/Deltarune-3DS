using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closetcutscene : MonoBehaviour {
	//public NewKrisController krisController;
	public dialogercutscenemaker dialoguer;
	public GameObject susie;
	public NewKrisController player;
	bool changed;
	void Start () {
		
	}

	void Update()
	{
		if (dialoguer.dialogues[1].message == "* There's nothing in here but old papers." && dialoguer.currentDialogueBox == null)
		{
			susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(-10.6f, susie.transform.position.y, susie.transform.position.z), Time.deltaTime * 3f);
			if (player.inisdeclosettexttrigger)
			{
				dialoguer.dialogues[0].message = "* That's weird...\n* I can't find a switch.";
				dialoguer.dialogues[1].message = "* Guess it's further in...";
				dialoguer.dialogues[2].message = "";
				dialoguer.RestartDialogue();
				player.inisdeclosettexttrigger = false;
			}
		}

		if (dialoguer.dialogues[1].message == "* Guess it's further in..." && dialoguer.currentDialogueBox == null)
		{
			susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(-5.66f, susie.transform.position.y, susie.transform.position.z), Time.deltaTime * 3f);
			if (player.inisdeclosettexttrigger)
			{
				dialoguer.dialogues[0].message = "* ...";
				dialoguer.dialogues[1].message = "* Uhhh... kind of big for a closet, huh...?";
				dialoguer.dialogues[2].message = "You'd think we'd have reached the end by now...";
				dialoguer.RestartDialogue();
				player.inisdeclosettexttrigger = false;
			}
		}

		if (dialoguer.dialogues[1].message == "* Uhhh... kind of big for a closet, huh...?" && dialoguer.currentDialogueBox == null)
		{
			susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(-0.57f, susie.transform.position.y, susie.transform.position.z), Time.deltaTime * 3f);
			if (player.inisdeclosettexttrigger)
			{
				dialoguer.dialogues[0].message = "* ...";
				dialoguer.dialogues[1].message = "* Hey, Kris.";
				dialoguer.dialogues[2].message = "* I think this closet's, uh...\n* Broken.";
				dialoguer.dialogues[3].message = "* There aren't any walls.";
				dialoguer.dialogues[4].message = "* ...";
				dialoguer.dialogues[5].message = "* Well, we've worked hard enough";
				dialoguer.dialogues[6].message = "* If Alphys wants chalk so bad, she can get it herself.";
				dialoguer.dialogues[7].message = "* Let's split.";
				dialoguer.RestartDialogue();
				player.inisdeclosettexttrigger = false;
			}
		}

		if (dialoguer.dialogues[1].message == "* Hey, Kris." && dialoguer.currentDialogueBox == null)
		{
			susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(-15.297f, susie.transform.position.y, susie.transform.position.z), Time.deltaTime * 3f);
			player.enabled = false;
			player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(-16.15f, player.transform.position.y, player.transform.position.z), Time.deltaTime * 3f);
			if (player.inisdeclosettexttriggerend)
			{
				dialoguer.dialogues[0].message = "* Wh-what the...!?";
				dialoguer.dialogues[1].message = "* Hey, this isn't funny!\n* LET US OUT!";
				dialoguer.dialogues[2].message = "* LET US...";
				dialoguer.dialogues[3].message = "* ...?";
				dialoguer.dialogues[4].message = "* The floor, it's...!";
				dialoguer.dialogues[5].message = "";
				dialoguer.dialogues[6].message = "";
				dialoguer.dialogues[7].message = "";
				dialoguer.RestartDialogue();
				player.inisdeclosettexttrigger = false;
			}
		}
	}
}
