using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int coins;
    public TMP_Text coinUI;
    public ShopItemSO[] shopItemsSO;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;
    public PlayerHealth playerHealth;
    public BasicShootScript shootScript;

    private void Start()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
            shopPanelsGO[i].SetActive(true);
        coins = playerHealth.currentCoins;
        LoadPanels();
        CheckPurchaseable();
    }
    private void Update()
    {
        coins = playerHealth.currentCoins;
        if (shootScript.weaponNum == 2)
        {
            shopPanelsGO[3].SetActive(false);
        }
    }

    //this function only serves the purpose of generating coins when we don't have the option to in game yet
    public void AddCoins()
    {
        coins++;
        coinUI.text = "Coins: " + coins.ToString();
        CheckPurchaseable();
    }

    public void CheckPurchaseable()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if (coins >= shopItemsSO[i].baseCost) //if I have enough money
                myPurchaseBtns[i].interactable = true;
            else
                myPurchaseBtns[i].interactable = false;
        }
    }


    public void PurchaseItem(int btnNo)
    {
        if (coins >= shopItemsSO[btnNo].baseCost)
        {
            playerHealth.currentCoins -= shopItemsSO[btnNo].baseCost;
            coinUI.text = "Coins: " + coins.ToString();
            CheckPurchaseable();
            //Unlock Item here
            switch (btnNo)
            {
                case 0: //rusty Armor
                    playerHealth.AddArmor(25.0f);
                    break;
                case 1: //used armor
                    playerHealth.AddArmor(50.0f);
                    break;
                case 2: // Shiny Armor
                    playerHealth.AddArmor(100.0f);
                    break;
                case 3: //Rate of Fire
                    shootScript.weaponNum++;
                    break;
                /*case 4: //AR
                    
                    break;
                case 5: //LMG
                    break;*/
                default:
                    break;
            }
        }
    }


    public void LoadPanels()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemsSO[i].title;
            shopPanels[i].descriptionTxt.text = shopItemsSO[i].description;
            shopPanels[i].costTxt.text = "Coins: " + shopItemsSO[i].baseCost.ToString();
        }
    }


}
