using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    Animator anim;
    bool isDead;


    void Update()
    {
        //THIS IS TO TEST THE DEATH 
        if (Input.GetKeyDown(KeyCode.M))
            Death();
       
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
    }
    public void TakeDamage(int damage, Vector3 hitPoint)
    {
        // If the enemy is dead
        if (isDead)
            // no need to take damage so exit the function.
            return;

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= damage;

        // If the current health is less than or equal to zero
        if (currentHealth <= 0)
        {
            // the enemy is dead.
            Death();
        }
    }
    void Death()
    {
        isDead = true;
        anim.SetBool("Death", isDead);// should trigger death animation# DOES NOT WORK
        Destroy(gameObject, 3f);//deletes enemy corpse after 3 seconds# WORKS
    }
}
