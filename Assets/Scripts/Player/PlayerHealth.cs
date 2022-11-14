using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth = 100;
    public int currentHealth;
    script_movement movement;
    Animator anim;
    bool isDead;
    
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        movement = GetComponent<script_movement>();
    }

    public void TakeDamage(int amount)
    {

        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }
    void Death()
    {
        isDead = true;
       // anim.SetTrigger("Die");
       //we dont have a playable character with a death yet
    }
}
