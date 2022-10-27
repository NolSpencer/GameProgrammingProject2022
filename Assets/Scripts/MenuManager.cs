using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject shopMenu;
    PlayerControls playerControls;
    bool ShopButtonPressed = false;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerControls.player.ShopMenu.started += ShopButton;
        playerControls.player.ShopMenu.canceled += ShopButton;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShopButtonPressed)
        {
            shopMenu.gameObject.SetActive(!shopMenu.gameObject.activeSelf);
        }
    }

    void ShopButton(InputAction.CallbackContext ctx)
    {
        ShopButtonPressed = ctx.ReadValueAsButton();
    }
}
