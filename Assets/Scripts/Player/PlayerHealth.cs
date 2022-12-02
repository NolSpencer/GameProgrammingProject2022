using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{
    public Image _healthbar;
    public Image _armorbar;
    public float startingHealth = 100;
    public float currentHealth;
    public float currentArmor = 0;

    public Text armorTextUI;
    public Text healthTextUI;
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
    private void Update()
    {
        HealthUIChange(currentHealth);
        ArmorUIChange(currentArmor);
    }
    void AddArmor(float amount)
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

    void ArmorUIChange(float armorValue)
    {
        _armorbar.fillAmount = armorValue;
        armorTextUI.text = armorValue.ToString();
    }
    //updates health UI
    void HealthUIChange(float healthValue)
    {
        _healthbar.fillAmount = healthValue;
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
