using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    public GameObject shopMenu;
    private GameObject player;
    public BasicShootScript shootScript;
    //disable shooting script on player (so no shooting while interacting with shopmenu
    //
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shootScript = GetComponent<BasicShootScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            shootScript.gameObject.SetActive(false);
            shopMenu.gameObject.SetActive(!shopMenu.gameObject.activeSelf);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            shootScript.gameObject.SetActive(false);
            shopMenu.gameObject.SetActive(!shopMenu.gameObject.activeSelf);
        }
    }
}
