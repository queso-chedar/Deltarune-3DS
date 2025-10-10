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
	public Collider2D Collider;
	public BattleEnemy BattleEnemyScript;
	public RectTransform rt;
	public GameObject battleBox;
	public GameObject tpgrazer;
	public GameObject soul;
	public Animator playeranimator;
	public Text DamageTextUI;
	public GameObject attack;
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
		EnemyHealth = BattleEnemyScript.EnemyHP;

		attackstartX = attack.transform.position.x;
		attackstartY = attack.transform.position.y;
		soulstartX = soul.transform.position.x;
		soulstartY = soul.transform.position.y;
		MoveFightBar = true;
	}

	// Update is called once per frame
	void Update()
	{
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
	void FixedUpdate()
	{
		if (MoveFightBar == true)
		{
			rt.anchoredPosition += new Vector2(velocidad, 0f);
		}
	{
      if (UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A) || (Input.GetKey(KeyCode.Z)))
		{
			playeranimator.Play("Slash", 0, 0f);
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
			DamageTextUI.text = Early.ToString();
			audioSource.PlayOneShot(snd_laz_c);
		}
		else if (other.CompareTag("Normal"))
		{
			EnemyHealth = EnemyHealth - Normal;
			DamageTextUI.text = Normal.ToString();
			audioSource.PlayOneShot(snd_laz_c);
		}
		else if (other.CompareTag("Good"))
		{
			EnemyHealth = EnemyHealth - Good;
			DamageTextUI.text = Good.ToString();
			audioSource.PlayOneShot(snd_laz_c);
		}
		else if (other.CompareTag("Perfect"))
		{
			EnemyHealth = EnemyHealth - Perfect;
			DamageTextUI.text = Perfect.ToString();
			audioSource.PlayOneShot(snd_laz_c);
			audioSource.PlayOneShot(snd_criticalswing);
		}
		else if (other.CompareTag("Late"))
		{
			EnemyHealth = EnemyHealth - Late;
			DamageTextUI.text = Late.ToString();
			audioSource.PlayOneShot(snd_laz_c);
		}
		else if (other.CompareTag("Miss"))
		{
			MoveFightBar = false;
			rt.anchoredPosition = new Vector2(283f, -3.5f);
		}
	}
}
	