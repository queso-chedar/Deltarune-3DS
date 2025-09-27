using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightBar : MonoBehaviour
{
	public Collider2D Collider;
	public BattleEnemy BattleEnemyScript;
	public bool CanAttack = true;
	private float movementSpeed = 5f;
	public int damage;
	public int FinalDamage;
	public bool On;
	public int enemymaxhp;

	// Use this for initialization
	void Start()
	{
		BattleEnemyScript.EnemyHP = enemymaxhp;
	}

	// Update is called once per frame
	void Update()
	{
		if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A) || (Input.GetKey(KeyCode.Z)))
		{
			Collider.enabled = true;
			On = false;
		}
		if (On == true)
		{
			transform.position += new Vector3(-2.5f, 0f, 0f);
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Early"))
		{
			Debug.Log("Tag Early Touched!");
		}
		else if (other.CompareTag("Normal"))
		{
			Debug.Log("Tag Normal Touched!");
		}
		else if (other.CompareTag("Good"))
		{
			Debug.Log("Tag Good Touched!");
		}
		else if (other.CompareTag("Perfect"))
		{
			Debug.Log("Tag Perfect Touched!");
		}
		else if (other.CompareTag("Late"))
		{
			Debug.Log("Tag Late Touched!");
		}
	}
	void EnableAttack()
	{
		On = true;
	}
	}
	
	