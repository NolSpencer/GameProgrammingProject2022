using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class script_movement : MonoBehaviour
{

    [SerializeField]
    private float maxPitch, minPitch;

    [SerializeField]
    private float movementSpeed, gravity, jumpForce;

    private PlayerControls controls;

    private GameObject cameraRotator;

    private CharacterController characterController;

    private Vector3 cameraRot;

    private Vector3 velocity;

    private Vector2 movementDirection;

    private Vector3 cameraForward, cameraRight, direction;

    private bool hasJumped = false;
    private void Awake()
    {
        controls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        cameraRotator = transform.GetChild(0).gameObject;
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
        controls.player.Jump.performed += JumpPressed;
        controls.player.Jump.canceled += JumpReleased;
    }
    // Update is called once per frame
    void Update()
    {
        //velocity = Vector3.zero;

        cameraForward = cameraRotator.transform.forward;
        cameraRight = cameraRotator.transform.right;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
        direction = (cameraRight * movementDirection.x) + (cameraForward * movementDirection.y);
        direction.y = 0.0f;
        velocity = direction * movementSpeed;

        //if (characterController.isGrounded)
        //    velocity.y = 0.0f;


        if (characterController.isGrounded)
        {
            //velocity = Vector3.zero;
            if(hasJumped)
                   velocity.y += jumpForce*Time.deltaTime;
        }
        velocity.y += gravity;

        characterController.Move(velocity*Time.deltaTime);

        UnityEngine.Debug.Log(velocity);
    }
    void Movement(InputAction.CallbackContext ctx)
    {
        movementDirection = ctx.ReadValue<Vector2>();
    }

    void CameraOrbit(InputAction.CallbackContext ctx)
    {
        Vector2 camDelta = ctx.ReadValue<Vector2>();
        cameraRot = cameraRotator.transform.localRotation.eulerAngles;
        cameraRot.y += camDelta.x;
        cameraRot.x += camDelta.y;
        cameraRot.x = Mathf.Clamp(cameraRot.x, minPitch, maxPitch);
        cameraRotator.transform.localEulerAngles = cameraRot;
    }
    void JumpPressed(InputAction.CallbackContext ctx)
    {
        hasJumped = ctx.ReadValueAsButton();
    }
    void JumpReleased(InputAction.CallbackContext ctx)
    {
       // hasJumped = ctx.ReadValueAsButton();
    }

}
