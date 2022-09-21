using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class script_cameraRotator : MonoBehaviour
{
    [SerializeField]
    private float maxPitch, minPitch;

    private PlayerControls controls;

    private Vector3 cameraRot;

    private void Awake()
    {
        controls = new PlayerControls();
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
        controls.player.CameraOrbit.performed += CameraOrbit;
    }
    // Update is called once per frame
    void Update()
    {
      
    }
    void CameraOrbit(InputAction.CallbackContext ctx)
    {
        Vector2 camDelta = ctx.ReadValue<Vector2>();
        cameraRot = transform.rotation.eulerAngles;
        UnityEngine.Debug.Log(camDelta);
        cameraRot.y += camDelta.x;
        cameraRot.x += camDelta.y;
        cameraRot.x = Mathf.Clamp(cameraRot.x, minPitch, maxPitch);
        transform.eulerAngles = cameraRot;
    }
}
