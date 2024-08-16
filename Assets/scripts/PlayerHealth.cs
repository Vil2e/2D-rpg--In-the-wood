using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Health
    {
        set
        {
            currentHealth = value;
        }

        get
        {
            return currentHealth;
        }
    }


    [SerializeField] PlayerController player;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D col;
    [SerializeField] Rigidbody2D rb;


    float currentHealth;
    float maxHealth = 12f;

    public delegate void HealthUpdate();
    //更新血量UI圖示
    public static event HealthUpdate OnhealthUpdate;

    private void Awake()
    {
        currentHealth = maxHealth;
    }


    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        animator.SetTrigger("hit");
        OnhealthUpdate();
        SFXManager.instance.PlayerHit();
        rb.AddForce(knockback);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.SetBool("isAlive", false);
            SFXManager.instance.PlayerDeath();
            col.enabled = false;
            rb.simulated = false;
            player.enabled = false;
        }


    }

    public void OnHit(float damage)
    {
        SFXManager.instance.PlayerHit();
        Health -= damage;
        OnhealthUpdate();
    }

    public void Recover(float value)
    {
        Health += value;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnhealthUpdate();

    }


}
