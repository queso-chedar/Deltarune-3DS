﻿using UnityEngine;
using UnityEngine.UI;

public class Battlebox : MonoBehaviour
{
    Animator animator;
    AnimationClip clip;
    public Battle_handle BattleHandler;
    float attacktimer = 100;
	private Image spriter_krisbuttons;
	private Image spriter_health;
	private Image spriter_name;

    void Awake()
    {
        //spriter_krisbuttons = BattleHandler.KirsFightButtons.GetComponent<UnityEngine.UI.Image>();
		//spriter_health = BattleHandler.KirsFightHealth.GetComponent<UnityEngine.UI.Image>();
		spriter_name = BattleHandler.KirsFightName.GetComponent<UnityEngine.UI.Image>();

        animator = GetComponent<Animator>();
        foreach (var c in animator.runtimeAnimatorController.animationClips)
        {
            if (c.name == "battlebox_destroy")
                clip = c;
        }
    }

    void Update()
    {
        attacktimer--;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (attacktimer <= 0)
        {
            animator.Play("battlebox_destroy");   

			BattleHandler.soul.SetActive(false);
			BattleHandler.tpgrazer.SetActive(false);
			BattleHandler.attack.SetActive(false); 
        }

        if (BattleHandler.soul.activeSelf == false)
        {
            BattleHandler.KirsFightButtons.SetActive(true);
            BattleHandler.KirsFightHealth.SetActive(true);
            BattleHandler.KirsFightName.SetActive(true); 
            //spriter_krisbuttons.color = new Color(spriter_krisbuttons.color.r, spriter_krisbuttons.color.g, spriter_krisbuttons.color.b, Mathf.MoveTowards(spriter_krisbuttons.color.a, 1f, 3 * Time.deltaTime));
            //spriter_health.color = new Color(spriter_health.color.r, spriter_health.color.g, spriter_health.color.b, Mathf.MoveTowards(spriter_health.color.a, 1f, 3 * Time.deltaTime));
            spriter_name.color = new Color(spriter_name.color.r, spriter_name.color.g, spriter_name.color.b, Mathf.MoveTowards(spriter_name.color.a, 1f, 3 * Time.deltaTime));
        }

        if (state.IsName("battlebox_destroy"))
        {
            float time = state.normalizedTime * clip.length;

            if (time >= clip.length)
            {
                attacktimer = 100;
                gameObject.SetActive(false);
            }
        }
    }
}