using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Enemy
{
    // Rhino action state
    private enum AttackState
    {
        Normal,
        SlowingDown,
        RotatingToShip,
        Charging
    }
    private AttackState attackState;

    // Horn transform
    public Transform horn;

    // Sound effects
    public AudioClip chargeUpSound, chargeDownSound;
    public AudioSource audioSource;

    // Aiming line 
    public SpriteRenderer line;
    public Color lineColor;

    // Charge cooldown
    protected float cooldown = 0f, targetSlowDownSpeed = 5f, targetSlowDownRate = 4f, 
        targetSpeedUpSpeed = 150f, targetSpeedUpRate = 3f, targetRotateSpeed = 150f, 
        targetRotateSpeedRate = 4f, moveSpeedHolder = 0f, rotateSpeedHolder = 0f,
        targetLineColorRate = 1.05f, targetHornSizeRate = 6f;

    // Override setup call
    public override void Setup(EnemyData data, Variant variant, Transform target)
    {
        base.Setup(data, variant, target);
        moveSpeedHolder = moveSpeed;
        rotateSpeedHolder = rotateSpeed;
    }

    // Override 

    // Override movement call
    public override void Move()
    {
        switch (attackState)
        {
            // Slow down enemy 
            case AttackState.SlowingDown:
                moveSpeed = Mathf.Lerp(moveSpeed, targetSlowDownSpeed, Time.deltaTime * targetSlowDownRate);
                if (moveSpeed - targetSlowDownSpeed <= 1f) attackState = AttackState.RotatingToShip;
                break;

            // Rotate to ship
            case AttackState.RotatingToShip:
                line.color = Color.Lerp(line.color, lineColor, Time.deltaTime * targetLineColorRate);
                rotateSpeed = Mathf.Lerp(rotateSpeed, targetRotateSpeed, Time.deltaTime * targetRotateSpeedRate);
                if (targetRotateSpeed - rotateSpeed <= 0.5f)
                {
                    rotateSpeed = 0f;
                    attackState = AttackState.Charging;
                    PlaySound(chargeDownSound);
                }
                break;

            // Charging
            case AttackState.Charging:
                horn.localScale = Vector2.Lerp(horn.localScale, Vector2.one, Time.deltaTime * targetHornSizeRate);
                moveSpeed = Mathf.Lerp(moveSpeed, targetSpeedUpSpeed, Time.deltaTime * targetSpeedUpRate);
                break;
        }

        base.Move();
    }

    // If force knockback received, cancel dash
    public override void Knockback(float amount, Vector3 origin, bool forceKnockback = false)
    {
        // Stun if force knockback true and charging
        if (forceKnockback && attackState == AttackState.Charging)
        {
            // Reset attack state
            DisableAttack();

            // Stun the enemy
            Stun(2f);

            // Base knockback
            base.Knockback(amount, origin, forceKnockback);
        }

        // Normal knockback
        else if (forceKnockback && attackState == AttackState.Normal)
        {
            base.Knockback(amount, origin, forceKnockback);
        }
    }

    // Called when colliding with ship
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if seeded
        if (IsSeeded()) return;

        // Get ship component
        Ship ship = collision.GetComponent<Ship>();

        // If ship not null, activate dash
        if (ship != null && attackState == AttackState.Normal)
        {
            PlaySound(chargeUpSound);
            attackState = AttackState.SlowingDown;
        }

        // If not ship, call base trigger
        else base.OnTriggerEnter2D(collision);
    }

    // Called when leaving ships range
    public void OnTriggerExit2D(Collider2D collision)
    {
        // Check if seeded
        if (IsSeeded()) return;

        // Get ship component
        Ship ship = collision.GetComponent<Ship>();

        // If ship not null, deactivate dash
        if (ship != null && attackState == AttackState.Charging)
        {
            DisableAttack();
        }
    }

    // Plays the audio source
    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = Settings.sound;
        if (audioSource.clip != null)
            audioSource.Play();
    }

    // Disable charging attack
    public void DisableAttack()
    {
        horn.localScale = Vector2.zero;
        line.color = new Color(lineColor.r, lineColor.g, lineColor.b, 0f);
        attackState = AttackState.Normal;
        moveSpeed = moveSpeedHolder;
        rotateSpeed = rotateSpeedHolder;
    }

    // When seeded, disable attack
    public override void SeedEnemy(Ship ship)
    {
        DisableAttack();
        base.SeedEnemy(ship);
    }
}
