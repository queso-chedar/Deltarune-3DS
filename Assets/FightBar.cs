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
	public Animator playeranimator;
	public bool On;
	public float velocidad = 4f;
	public RectTransform rt;
    private float attackstartX;
    private float attackstartY;
    private float soulstartX;
    private float soulstartY;
    public GameObject battleBox;
    public GameObject tpgrazer;
    public GameObject soul;
    public GameObject attack;

	// Use this for initialization
	void Start()
	{
		EnemyHealth = BattleEnemyScript.EnemyHP;

        attackstartX = attack.transform.position.x;
        attackstartY = attack.transform.position.y;
        soulstartX = soul.transform.position.x;
        soulstartY = soul.transform.position.y;
	}

	// Update is called once per frame
	void Update()
	{
		if (On == true)
		{
			rt.anchoredPosition += new Vector2(velocidad, 0f);
		}
		BattleEnemyScript.EnemyHP = EnemyHealth;

		if (playeranimator.GetCurrentAnimatorStateInfo(0).IsName("Slash") && (playeranimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !playeranimator.IsInTransition(0)))
		{
			attack.transform.position = new Vector3(attackstartX, attackstartY, 0);
			soul.transform.position = new Vector3(soulstartX, soulstartY, 0);
			playeranimator.Play("FightIdle", 0, 0f);
			battleBox.SetActive(true);
			soul.SetActive(true);
			tpgrazer.SetActive(true);
			attack.SetActive(true);
		}
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
			playeranimator.Play("Slash", 0, 0f);
			Collider.enabled = true;
			On = false;
		}
	}
	}
	
	