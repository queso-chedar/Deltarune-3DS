using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notehitter : MonoBehaviour
{
    public enum _side
    {
        Left,
        Right
    }

	public _side side;


	private Animator note_effectanim;
	public GameObject note_effect;


	private Animator kripyanim;
	public GameObject krispy;


	public
	void Start()
	{
		note_effectanim = note_effect.GetComponent<Animator>();
		kripyanim = krispy.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (note_effect.transform.localScale.x >= 0)
			note_effect.transform.localScale -= new Vector3(0.1f, 0, 0);
			
		if ((UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.A) || Input.GetKeyDown(KeyCode.Z)) && side == _side.Left || (UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.B) || Input.GetKeyDown(KeyCode.X)) && side == _side.Right)
		{
			if (note_effectanim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !note_effectanim.IsInTransition(0))
			{
				note_effectanim.Play("effect", 0, 0f);
				note_effect.transform.localScale = new Vector3(1, 1, 1);




				if (side == _side.Left)
				{
					if (kripyanim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !kripyanim.IsInTransition(0))
					{
						kripyanim.Play("guitar_low", 0, 0f);
					}
				}
				else
				{
					if (kripyanim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !kripyanim.IsInTransition(0))
					{
						kripyanim.Play("guitar_high", 0, 0f);
					}
				}




			}
		}
	}
	
	public void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("leftnote") && side == _side.Left || collision.CompareTag("rightnote") && side == _side.Right)
		{
			if ((UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.A) || Input.GetKeyDown(KeyCode.Z)) && side == _side.Left || (UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.B) || Input.GetKeyDown(KeyCode.X)) && side == _side.Right)
			{
				//Debug.Log("note hit!");
				Destroy(collision.gameObject);
			}
		}
	}
}
