using System.Collections;
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
    private void Awake()
    {
        controls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        cameraRotator = transform.GetChild(2).gameObject;
        Capsule = transform.GetChild(0).gameObject;
        Cube = transform.GetChild(1).gameObject;
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
        controls.player.Jump.performed += JumpButton;
        controls.player.Jump.canceled += JumpButton;
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
        //if (movementDirection.x > 0.0f || movementDirection.y > 0.0f)
        //{
            //Vector3 playerRot = cameraRotator.transform.eulerAngles;
            //playerRot.y = 0.0f;
       //     Capsule.transform.rotation = cameraRotator.transform.rotation;
       //     Cube.transform.rotation = cameraRotator.transform.rotation;

       // }

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
        cameraRot.y += camDelta.x*mouseSensitivity;
        cameraRot.x += camDelta.y*mouseSensitivity;
        cameraRot.x = Mathf.Clamp(cameraRot.x, minPitch, maxPitch);
        cameraRotator.transform.localEulerAngles = cameraRot;
    }
    void JumpButton(InputAction.CallbackContext ctx)
    {
        jumpPressed = ctx.ReadValueAsButton();
    }
}
