using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class battleselect : MonoBehaviour
{
    public GameObject battleBox;
    public GameObject soul;
    public GameObject playerrow;
    public GameObject self;
    public GameObject krisaimhead;
    public GameObject krispressattack;
    public GameObject speakertext;
    public GameObject speakericon;
    public GameObject attack;
    public GameObject battle_helper;
    public GameObject EventSystemthingy;
    public GameObject Player;
    public GameObject battleaimer;
    public GameObject battlehitter;
    [SerializeField] private Button targetButton;

    private float attackstartX;
    private float attackstartY;
    private float soulstartX;
    private float soulstartY;
    private Animator playeranimator;

    Text changetext;

    void Start()
    {
        attackstartX = attack.transform.position.x;
        attackstartY = attack.transform.position.y;
        soulstartX = soul.transform.position.x;
        soulstartY = soul.transform.position.y;
        playeranimator = Player.GetComponentInChildren<Animator>();
        changetext = speakertext.GetComponent<Text>();
    }

    public void ButtonPressed()
    {
        playeranimator.Play("fightreadyup", 0, 0f);
        //attack.transform.position = new Vector3(attackstartX, attackstartY, 0);
        //soul.transform.position = new Vector3(soulstartX, soulstartY, 0);
        //battleBox.SetActive(true);
        playerrow.SetActive(false);
        //soul.SetActive(true);
        //self.SetActive(true);
        speakertext.SetActive(false);
        speakericon.SetActive(false);
        battlehitter.SetActive(false);
        
        krispressattack.SetActive(true);
        krisaimhead.SetActive(true);

        battleaimer.SetActive(true);
        battlehitter.SetActive(true);

        //attack.SetActive(true);
        //battle_helper.SetActive(true);
        EventSystemthingy.SetActive(false);
        //changetext.text = "poo poo pee pee";
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == targetButton.gameObject)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Z))
            {
                ButtonPressed();
            }
        }
    }
}
