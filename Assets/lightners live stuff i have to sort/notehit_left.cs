using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notehit_left : MonoBehaviour
{

	public Animator note_effect;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	
	public void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("leftnote"))
		{
			if (UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.A) || Input.GetKeyDown(KeyCode.Z))
			{
				Debug.Log("note hit!");
				Destroy(collision.gameObject);


				if (note_effect.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !note_effect.IsInTransition(0))
				{
					note_effect.Play("effect", 0, 0f);
				}
			}
		}
	}
}
