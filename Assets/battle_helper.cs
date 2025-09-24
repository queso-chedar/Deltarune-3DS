﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class battle_helper : MonoBehaviour
{
    public float attack_length;
    private float timer = -1;


    //thy objects!
    public GameObject battleBox;
    public GameObject soul;
    public GameObject playerrow;
    public GameObject speakertext;
    public GameObject speakericon;
    //public GameObject attack;
    public GameObject battlehelper;
    public GameObject EventSystemthingy;

    void Update()
    {
        if (timer == -1)
        {
            timer = attack_length;
        }
        timer--;

        if (timer <= 0)
        {
            Debug.Log("Button pressed!");
            //change the battleBox animation to battlebox_destroy here
            battleBox.GetComponent<Animator>().Play("battlebox_destroy");
            playerrow.SetActive(true);
            soul.SetActive(false);
            speakertext.SetActive(true);
            speakericon.SetActive(true);
            //attack.SetActive(false);
            EventSystemthingy.SetActive(true);
            battlehelper.SetActive(false);
            timer = -1;
        }
    }
}