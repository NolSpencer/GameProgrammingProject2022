﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class script_movement : MonoBehaviour
{

    [SerializeField]
    private float maxPitch, minPitch;

    [SerializeField]
    [Range(0f, 4f)] 
    private float mouseSensitivity = 1.0f;

    [SerializeField]
    private float movementSpeed, gravity, jumpForce, terminalVelocity, groundedGravity;

    private PlayerControls controls;

    private GameObject cameraRotator;

    private GameObject Capsule, Cube;

    private CharacterController characterController;

    private Vector3 cameraRot;

    private Vector3 velocity;

    private Vector2 movementDirection;

    private Vector3 cameraForward, cameraRight, direction;

    private float yVelocity;

    private bool jumpPressed = false;

    private bool hasJumped = false;

    private bool hasFired = false;

    private bool hasHitSomething = false;

    private RaycastHit hit;
    private void Awake()
    {
        controls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        cameraRotator = transform.GetChild(1).gameObject;
        Capsule = transform.GetChild(0).gameObject;
        Cube = transform.GetChild(0).GetChild(0).gameObject;
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        controls.player.Movement.performed += Movement;
        controls.player.CameraOrbit.performed += CameraOrbit;
        controls.player.Jump.started += JumpButton;
        controls.player.Jump.canceled += JumpButton;
        controls.player.Fire.started += Fire;
        controls.player.Fire.canceled += Fire;
    }
    // Update is called once per frame
    void Update()
    {
        //velocity = Vector3.zero;

        cameraForward = cameraRotator.transform.forward;
        cameraRight = cameraRotator.transform.right;
        cameraForward.Normalize();
        cameraRight.Normalize();

        direction = (cameraRight * movementDirection.x) + (cameraForward * movementDirection.y);
        direction.y = 0;
        direction.Normalize();
        
        velocity = Vector3.Lerp(velocity, direction * movementSpeed, .2f);
        if (movementDirection.x > 0.0f || movementDirection.y > 0.0f)
        {
            Vector3 playerRot = cameraRotator.transform.eulerAngles;
            playerRot.x = 0.0f;
            playerRot.z = 0.0f;
            Capsule.transform.rotation = Quaternion.Lerp(Capsule.transform.rotation, Quaternion.Euler(playerRot),.4f);
            //Cube.transform.rotation = Quaternion.Euler(playerRot);

        }

        if (characterController.isGrounded)
        {
            yVelocity = groundedGravity;
            if (jumpPressed && !hasJumped)
            {
                hasJumped = true;
                yVelocity = jumpForce;
            }
            else
            {
                hasJumped = false;
            }
        }
        if(yVelocity > terminalVelocity)
            yVelocity += gravity;

        velocity.y = yVelocity;

        characterController.Move(velocity*Time.deltaTime);

    }
    void Movement(InputAction.CallbackContext ctx)
    {
        movementDirection = ctx.ReadValue<Vector2>();
    }

    void CameraOrbit(InputAction.CallbackContext ctx)
    {
        Vector2 camDelta = ctx.ReadValue<Vector2>();
        cameraRot = cameraRotator.transform.eulerAngles;
        cameraRot.y += camDelta.x*mouseSensitivity;
        cameraRot.x += camDelta.y*mouseSensitivity;
        cameraRot.x = Mathf.Clamp(cameraRot.x, minPitch, maxPitch);
        cameraRotator.transform.rotation = Quaternion.Euler(cameraRot);
        //UnityEngine.Debug.Log(cameraRotator.transform.rotation);
    }
    void JumpButton(InputAction.CallbackContext ctx)
    {
        jumpPressed = ctx.ReadValueAsButton();
    }
    void Fire(InputAction.CallbackContext ctx)
    {
        hasFired = ctx.ReadValueAsButton();
        if(!hasFired)
        {
           hasHitSomething = Physics.Raycast(transform.position, cameraRotator.transform.forward, out hit, 200.0f, 0);
           Debug.DrawRay(transform.position, cameraRotator.transform.forward);
        }
    }
}