using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razorclaw : Boss
{
    public List<Transform> saws;
    public float sawRotateSpeed;
    public Animator animator;
    public AnimationClip normalAnimation;
    public List<AnimationClip> attacks;
    private float cooldown = 10f;
    
    public override void Setup()
    {
        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        base.Setup();
    }

    public void Update()
    {
        // Check if paused
        if (Dealer.isOpen && animator.enabled) animator.enabled = false;
        else if (!Dealer.isOpen && !animator.enabled) animator.enabled = true;

        // Rotate the saws
        foreach (Transform saw in saws)
            saw.Rotate(Vector3.forward, sawRotateSpeed * Time.deltaTime);

        // Random attacks
        if (cooldown <= 0f)
        {
            animator.SetInteger("Attack", Random.Range(0, attacks.Count));
            cooldown = 3f;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }
}
