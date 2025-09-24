﻿using UnityEngine;

public class Battlebox : MonoBehaviour
{
    Animator animator;
    AnimationClip clip;

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
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

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