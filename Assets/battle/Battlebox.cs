﻿using UnityEngine;

public class Battlebox : MonoBehaviour
{
    Animator animator;
    AnimationClip clip;
    public Battle_handle BattleHandler;
    float attacktimer = 100;
    void Awake()
    {
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

        if (state.IsName("battlebox_destroy"))
        {
            float time = state.normalizedTime * clip.length;

            if (time >= clip.length)
            {
                gameObject.SetActive(false);
            }
        }
    }
}