using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject shopMenu;
    PlayerControls playerControls;
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            shopMenu.gameObject.SetActive(!shopMenu.gameObject.activeSelf);
        }
    }

}
