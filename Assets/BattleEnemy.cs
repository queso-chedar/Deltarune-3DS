using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour
{
	public bool HasMercy;
	public string EnemyName = "Debug enemy";
	public Text DialogueBoxText;
	public Text HPText;
	public bool WinBattle;
	public enemyhealth _enemyhealth;
	public bool CanSpare;

	[Header("Estadisticas")]
	public int EnemyHP = 100;
	public int Mercy;
	public int EnemyDamage;
	public int EnemyDefense;
	public string Description;

	void Start()
	{
		DialogueBoxText.text = "* " + EnemyName + " Drew near!";
	}

	// Update is called once per frame
	void Update()
	{
		if (EnemyHP <= 0)
		{
			_enemyhealth.Finishtbattlebool = true;
		}
		HPText.text = "EnemyHP: " + EnemyHP;

		if (Mercy >= 100)
		{
			Mercy = 100;
		}
	}
	public void CheckEnemy()
	{
		DialogueBoxText.text = "* " + EnemyName + " - " + EnemyDamage + " De daño y " + EnemyDefense + " De defensa - " + Description;
	}
}
