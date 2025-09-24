using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class battleselect : MonoBehaviour
{
    //[SerializeField] private string room;
    public GameObject battleBox;
    public GameObject soul;
    public GameObject playerrow;
    public GameObject self;
    public GameObject speakertext;
    public GameObject speakericon;
    public GameObject attack;
    public GameObject battle_helper;
    public GameObject EventSystemthingy;

    [SerializeField] private Button targetButton;


    private float attackstartX;
    private float attackstartY;

    
    private float soulstartX;
    private float soulstartY;

    public void Start()
    {
        attackstartX = attack.transform.position.x;
        attackstartY = attack.transform.position.y;

        soulstartX = soul.transform.position.x;
        soulstartY = soul.transform.position.y;
    }

    public void ButtonPressed()
    {
        attack.transform.position = new Vector3(attackstartX, attackstartY, 0);
        soul.transform.position = new Vector3(soulstartX, soulstartY, 0);
        Debug.Log("Button pressed!");
        battleBox.SetActive(true);
        playerrow.SetActive(false);
        soul.SetActive(true);
        self.SetActive(true);
        speakertext.SetActive(false);
        speakericon.SetActive(false);
        attack.SetActive(true);
        battle_helper.SetActive(true);
        EventSystemthingy.SetActive(false);
        //SceneManager.LoadScene(room);
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
