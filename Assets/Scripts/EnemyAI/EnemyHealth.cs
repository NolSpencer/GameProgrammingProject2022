using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth = 100;
    public float currentHealth;
    public GameObject coin;
    public GameObject healthPack;
    Animator anim;
    GameObject spwn;
    bool isDead;
    float dropTimer;
    public int drop;


    void Update()
    {
        //THIS IS TO TEST THE DEATH 
        if (Input.GetKeyDown(KeyCode.M))
            Death();
        if (dropTimer > 0f)
        {
            dropTimer -= Time.deltaTime;
        }
        else if (dropTimer < 0f && isDead)
        {
            if (drop <= 2)
            {
                Instantiate(healthPack, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
                dropTimer = 0f;
            }
            else if (drop >= 3 && drop <= 6)
            {
                Instantiate(coin, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
                dropTimer = 0f;
            }
        }
       
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
        drop = Random.Range(1, 10);
        spwn = GameObject.FindGameObjectWithTag("Spawner");
    }
    public void TakeDamage(float damage, Vector3 hitPoint)
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
        anim.SetBool("Death", isDead);// should trigger death animation# DOES NOT WORK
        dropTimer = 2.9f;
        Destroy(gameObject, 3f);//deletes enemy corpse after 3 seconds# WORKS
        isDead = true;
        spwn.GetComponent<Spawner>().EnemyKilled();
    }
}
