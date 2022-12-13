using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public Image _healthbar;
    public Image _armorbar;
    public float startingHealth = 100;
    public float currentHealth;
    public float currentArmor = 0;
    public int currentCoins = 0;

    public AudioSource playerAudio;
    public Text armorTextUI;
    public Text healthTextUI;
    public TMP_Text coins;
    script_movement movement;
    Animator anim;
    bool isDead;
    
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        movement = GetComponent<script_movement>();
        playerAudio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        HealthUIChange(currentHealth);
        ArmorUIChange(currentArmor);
        coins.text = "Coins " + currentCoins.ToString();
    }
    public void AddArmor(float amount)
    {
        if(currentArmor + amount > 100.0f)
        {
            currentArmor = 100.0f;
        }
        else
        {
            currentArmor += amount;
        }
        
    }

    public void AddHealth(float amount)
    {
        if (currentHealth + amount > 100.0f)
        {
            currentHealth = 100.0f;
        }
        else
        {
            currentHealth += amount;
        }

    }

    public void CollectCoins(int amount)
    {
        currentCoins += amount;
    }

    void ArmorUIChange(float armorValue)
    {
        _armorbar.fillAmount = (armorValue / 100.0f);
        armorTextUI.text = armorValue.ToString();
    }
    //updates health UI
    void HealthUIChange(float healthValue)
    {
        _healthbar.fillAmount = (healthValue / 100.0f);
        healthTextUI.text = healthValue.ToString();
    }
    public void TakeDamage(float amount)
    {
        //This is the damage that will be delt to the players health after taking away from the armor
        float carryoverDamage;
        if (currentArmor > 0)
        {
            if (currentArmor - amount < 0)
            {
                carryoverDamage = System.Math.Abs(currentArmor - amount);
                currentArmor = 0;
                currentHealth -= amount;
                playerAudio.Play();
            }
            else
            {
                currentArmor -= amount;
            }
        }
        else
        {
            currentHealth -= amount;
        }

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
