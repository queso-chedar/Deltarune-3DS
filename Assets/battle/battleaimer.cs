using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battleaimer : MonoBehaviour
{
    public float speed = 80;
    public GameObject enemyhealthmanager;
    public GameObject battleBox;
    public GameObject soul;
    public GameObject self;
    public GameObject attack;
    public GameObject battle_helper;
	public GameObject enemy;
    private float attackstartX;
    private float attackstartY;
    private float soulstartX;
    private float soulstartY;
    private float startX;
    private float startY;
    private float timertime = 50;
    private float timer;
    private bool starttrans;
	public GameObject battlehitter;

    public GameObject Player;
	private Animator playeranimator;
	private Animator enemyanimator;
    private enemyhealth eh;
    Text changetext;
    public GameObject funnytext;
	

    void Start()
    {
        changetext = funnytext.GetComponent<Text>();
        attackstartX = attack.transform.position.x;
        attackstartY = attack.transform.position.y;
        startX = transform.position.x;
        startY = transform.position.y;
        soulstartX = soul.transform.position.x;
        soulstartY = soul.transform.position.y;
        playeranimator = Player.GetComponentInChildren<Animator>();
		enemyanimator = enemy.GetComponentInChildren<Animator>();
        eh = enemyhealthmanager.GetComponent<enemyhealth>();
        timer = timertime;
        starttrans = false;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.A) && !starttrans || Input.GetKeyDown(KeyCode.Z) && !starttrans)
        {
            playeranimator.Play("Slash", 0, 0f);
			enemyanimator.Play("hurt", 0, 0f);
			if (eh != null)
			{
				eh.health -= 50;
				starttrans = true;
				speed = 0;
			}
        }

        if (starttrans)
        {
            timer -= 1;
            //Debug.Log(timer.ToString());
            if (timer <= 0)
            {
                playeranimator.Play("FightIdle", 0, 0f);
				enemyanimator.Play("idle", 0, 0f);
                attack.transform.position = new Vector3(attackstartX, attackstartY, 0);
                soul.transform.position = new Vector3(soulstartX, soulstartY, 0);
				transform.position = new Vector3(startX, startY, 0);
                battleBox.SetActive(true);
                soul.SetActive(true);
                attack.SetActive(true);
				battlehitter.SetActive(false);
                battle_helper.SetActive(true);
                changetext.text = "poo poo pee pee";
				timer = timertime;
				starttrans = false;
				speed = 80;
                self.SetActive(false);
            }
        }
    }
}
