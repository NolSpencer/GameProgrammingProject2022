using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    public GameObject shopMenu;
    private GameObject player;
    private GameObject gun;
    private StarterAssetsInputs inputs;
    public BasicShootScript shootScript;
    //disable shooting script on player (so no shooting while interacting with shopmenu
    //
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gun = GameObject.FindGameObjectWithTag("Gun");
        shootScript = gun.GetComponent<BasicShootScript>();
        inputs = player.GetComponent<StarterAssetsInputs>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            shootScript.enabled = false;
            shopMenu.SetActive(true);
            inputs.cursorLocked = false;
            Cursor.visible = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            shootScript.enabled = true;
            shopMenu.SetActive(false);
            inputs.cursorLocked = true;
            Cursor.visible = false;
        }
    }
}
