using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 10;               // The amount of health taken away per attack.


    Animator anime;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.

    void Awake()
    {
        // Setting up the references
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anime = GetComponent<Animator>();
    }
    void OnTriggerEnter(Collider other)
    {
        // If the entering collider is the player
        if (other.gameObject == player)
        {
            // the player is in range.
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the player
        if (other.gameObject == player)
        {
            // the player is no longer in range.
            playerInRange = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Add the time since update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive
        if (timer >= .35f && playerInRange/* && enemyHealth.currentHealth > 0*/)
        {
            // attack
            // anim.SetTrigger("Attack");
            Attack();
        }

        // If the player has zero or less health
        /* WE DONT HAVE A PLAYABLE CHARACTER ANIMATION YET
        if (playerHealth.currentHealth <= 0)
        {
            
            //anime.SetTrigger("PlayerDead");
        }
        */
    }
    void Attack()
    {
        // Reset the timer.
        timer = 0f;

        // If the player has health to lose
        if (playerHealth.currentHealth > 0)
        {
            // damage the player.
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
