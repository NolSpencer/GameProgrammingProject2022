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

    private GameObject Capsule, Cube, cam;

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

    private float rayLength;
    
    private RaycastHit camRayHit;

    private Vector3 camInitialPos;

    private int rayLayer;

    private RaycastHit hit;
    private void Awake()
    {
        controls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        cameraRotator = transform.GetChild(1).gameObject;
        cam = cameraRotator.transform.GetChild(0).gameObject;
        Capsule = transform.GetChild(0).gameObject;
        Cube = transform.GetChild(0).GetChild(0).gameObject;
        rayLength = cam.transform.localPosition.z;
        rayLayer = LayerMask.GetMask("CamRay");
        camInitialPos = cam.transform.localPosition;
        //minPitch -= 360.0f;
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
        if (movementDirection.y > 0.0f)
        {
            Vector3 playerRot = cameraRotator.transform.eulerAngles;
            playerRot.x = 0.0f;
            playerRot.z = 0.0f;
            //float forwardOrBack = movementDirection.y > 0.0f ? 1.0f : -1.0f;
            Capsule.transform.rotation = Quaternion.Lerp(Capsule.transform.rotation, Quaternion.Euler(playerRot),.4f);
            //Cube.transform.rotation = Quaternion.Euler(playerRot);

        }
        if (movementDirection.y < 0.0f)
        {
            Vector3 playerRot = cameraRotator.transform.eulerAngles;
            playerRot.x = 0.0f;
            playerRot.z = 0.0f;
            playerRot.y -= 180.0f;
            //float forwardOrBack = movementDirection.y > 0.0f ? 1.0f : -1.0f;
            Capsule.transform.rotation = Quaternion.Lerp(Capsule.transform.rotation, Quaternion.Euler(playerRot), .4f);
            //Cube.transform.rotation = Quaternion.Euler(playerRot);

        }
        if (movementDirection.x > 0.0f)
        {
            Vector3 playerRot = cameraRotator.transform.eulerAngles;
            playerRot.x = 0.0f;
            playerRot.z = 0.0f;
            playerRot.y += 90.0f;
            //float forwardOrBack = movementDirection.y > 0.0f ? 1.0f : -1.0f;
            Capsule.transform.rotation = Quaternion.Lerp(Capsule.transform.rotation, Quaternion.Euler(playerRot), .4f);
            //Cube.transform.rotation = Quaternion.Euler(playerRot);

        }
        if (movementDirection.x < 0.0f)
        {
            Vector3 playerRot = cameraRotator.transform.eulerAngles;
            playerRot.x = 0.0f;
            playerRot.z = 0.0f;
            playerRot.y -= 90.0f;
            //float forwardOrBack = movementDirection.y > 0.0f ? 1.0f : -1.0f;
            Capsule.transform.rotation = Quaternion.Lerp(Capsule.transform.rotation, Quaternion.Euler(playerRot), .4f);
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

        cameraCollision();

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
        if(cameraRot.x>=270.0f)
        {
            cameraRot.x -= 360.0f;
        }
        if(cameraRot.x >= maxPitch)
        {
            cameraRot.x = maxPitch;
        }
        if(cameraRot.x <= minPitch)
        {
            cameraRot.x = minPitch;
        }
        cameraRotator.transform.rotation = Quaternion.Euler(cameraRot);
        //UnityEngine.Debug.Log(cameraRotator.transform.rotation);
    }
    void cameraCollision()
    {
        Ray camRay = new Ray(cameraRotator.transform.position, (cameraRotator.transform.forward * -1.0f));
        Vector3 camRayStart = cameraRotator.transform.position;
        //camRayStart.y += 5.0f;
        //UnityEngine.Debug.Log(cam.transform.localPosition);
        UnityEngine.Debug.DrawRay(camRayStart, cameraRotator.transform.forward * -1.0f, Color.red);
        float distanceFromPlayer = rayLength;
        Vector3 temp = cam.transform.localPosition;
        if (Physics.Raycast(camRay, out camRayHit, Mathf.Abs(rayLength), rayLayer))
        {
            temp = camRayHit.point;
            //UnityEngine.Debug.Log(camRayHit.point);
        }
        else
        {
            temp = camRay.GetPoint(Mathf.Abs(rayLength));
        }
        //UnityEngine.Debug.Log(camRayHit.point);
        //else
        //{
        //    temp = transform.TransformPoint(camInitialPos);
        //}
        //if(cam.transform.localPosition.z < rayLength)
        //{
        //    distanceFromPlayer = rayLength;
        //}

        Vector3 offsetLocal = Vector3.zero;
        offsetLocal.y = 2.0f;

        cam.transform.position = Vector3.Lerp(cam.transform.position, temp, 0.4f);
        cam.transform.localPosition += offsetLocal;
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
