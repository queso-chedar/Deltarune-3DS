using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightBar : MonoBehaviour
{
	[Header("Stuff")]
	public bool CanAttack = true;
	public int EnemyHealth;
	public bool MoveFightBar;
	public float velocidad = 4f;
	[Header("Sounds")]
	public AudioSource audioSource;
	public AudioClip snd_criticalswing;
	public AudioClip snd_laz_c;
	[Header("Shitty References")]
	public Battle_handle BattleHandler;
	public Collider2D Collider;
	public RectTransform rt;

	[Header("Debug Damage")]
	public int Early = 16;
	public int Normal = 29;
	public int Good = 36;
	public int Perfect = 50;
	public int Late = 17;

	//-------Private-Variables-------//
	private float attackstartX;
	private float attackstartY;
	private float soulstartX;
	private float soulstartY;
	private float movementSpeed = 5f;

	// Use this for initialization
	void Start()
	{
		EnemyHealth = BattleHandler.BattleEnemyScript.EnemyHP;

		attackstartX = BattleHandler.attack.transform.position.x;
		attackstartY = BattleHandler.attack.transform.position.y;
		soulstartX = BattleHandler.soul.transform.position.x;
		soulstartY = BattleHandler.soul.transform.position.y;
		MoveFightBar = true;
	}

	// Update is called once per frame
	void Update()
	{
		BattleHandler.BattleEnemyScript.EnemyHP = EnemyHealth;

		if (BattleHandler.playeranimator.GetCurrentAnimatorStateInfo(0).IsName("Slash") && (BattleHandler.playeranimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !BattleHandler.playeranimator.IsInTransition(0)))
		{
			BattleHandler.attack.transform.position = new Vector3(attackstartX, attackstartY, 0);
			BattleHandler.soul.transform.position = new Vector3(soulstartX, soulstartY, 0);
			BattleHandler.playeranimator.Play("FightIdle", 0, 0f);
			BattleHandler.battleBox.SetActive(true);
			BattleHandler.soul.SetActive(true);
			BattleHandler.tpgrazer.SetActive(true);
			BattleHandler.attack.SetActive(true);
		}
	}
	void FixedUpdate()
	{
		if (MoveFightBar == true)
		{
			rt.anchoredPosition += new Vector2(velocidad, 0f);
		}
	{
      if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A) || (Input.GetKeyDown(KeyCode.Z)))
		{
			BattleHandler.playeranimator.Play("Slash", 0, 0f);
			Collider.enabled = true;
			MoveFightBar = false;
		}
	}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Early"))
		{
			EnemyHealth = EnemyHealth - Early;
			BattleHandler.DamageTextUI.text = Early.ToString();
			audioSource.PlayOneShot(snd_laz_c);
		}
		else if (other.CompareTag("Normal"))
		{
			EnemyHealth = EnemyHealth - Normal;
			BattleHandler.DamageTextUI.text = Normal.ToString();
			audioSource.PlayOneShot(snd_laz_c);
		}
		else if (other.CompareTag("Good"))
		{
			EnemyHealth = EnemyHealth - Good;
			BattleHandler.DamageTextUI.text = Good.ToString();
			audioSource.PlayOneShot(snd_laz_c);
		}
		else if (other.CompareTag("Perfect"))
		{
			EnemyHealth = EnemyHealth - Perfect;
			BattleHandler.DamageTextUI.text = Perfect.ToString();
			audioSource.PlayOneShot(snd_laz_c);
			audioSource.PlayOneShot(snd_criticalswing);
		}
		else if (other.CompareTag("Late"))
		{
			EnemyHealth = EnemyHealth - Late;
			BattleHandler.DamageTextUI.text = Late.ToString();
			audioSource.PlayOneShot(snd_laz_c);
		}
		else if (other.CompareTag("Miss"))
		{
			MoveFightBar = false;
			rt.anchoredPosition = new Vector2(283f, -3.5f);
		}
	}
}
	