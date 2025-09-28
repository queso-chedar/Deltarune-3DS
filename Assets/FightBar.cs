using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightBar : MonoBehaviour
{
	public Collider2D Collider;
	public BattleEnemy BattleEnemyScript;
	public bool CanAttack = true;
	private float movementSpeed = 5f;
	public int EnemyHealth;
	public bool On;
	public float velocidad = 4f;
	public RectTransform rt;

	// Use this for initialization
	void Start()
	{
		EnemyHealth = BattleEnemyScript.EnemyHP;
	}

	// Update is called once per frame
	void Update()
	{
		if (On == true)
		{
			rt.anchoredPosition += new Vector2(velocidad, 0f);
		}
		BattleEnemyScript.EnemyHP = EnemyHealth;
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Early"))
		{
			EnemyHealth = EnemyHealth - 16;
		}
		else if (other.CompareTag("Normal"))
		{
			EnemyHealth = EnemyHealth - 29;
		}
		else if (other.CompareTag("Good"))
		{
			EnemyHealth = EnemyHealth - 36;
		}
		else if (other.CompareTag("Perfect"))
		{
			EnemyHealth = EnemyHealth - 50;
		}
		else if (other.CompareTag("Late"))
		{
			EnemyHealth = EnemyHealth - 17;
		}
		else if (other.CompareTag("Miss"))
		{
			On = false;
			rt.anchoredPosition = new Vector2(283f, -3.5f);
		}
	}
	void FixedUpdate()
	{
      if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A) || (Input.GetKey(KeyCode.Z)))
		{
			Collider.enabled = true;
			On = false;
		}
	}
	}
	
	