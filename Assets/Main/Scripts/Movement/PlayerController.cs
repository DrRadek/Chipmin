using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cameraRotationHelperX;
    [SerializeField] GameObject cameraRotationHelperY;
    [SerializeField] GameObject movementRotationHelper;
    [SerializeField] TriggerCounter floorTrigger;
    [SerializeField] Camera playerCamera;
    [SerializeField] Light globalLight;
    [SerializeField] BoxCollider mapRange;

    Rigidbody rb;

    Vector3 dir = Vector3.zero;

    Controls controls;
    Controls.PlayerActions playerControls;

    PlayerStats playerStats;

    Vector3 defaultPos;
    Quaternion defaultRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        controls = new Controls();
        playerControls = controls.Player;
        playerControls.Mouse.performed += MouseChangeCallback;
        playerControls.CameraScroll.performed += CameraScrollCallback;
        playerControls.Jump.performed += JumpCallback;
        playerControls.CollectorLeft.performed += CollectorLeftCallback;
        playerControls.CollectorRight.performed += CollectorRightCallback;
    }

    private void Start()
    {
        playerStats = PlayerStats.instance;
        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    private void CollectorLeftCallback(InputAction.CallbackContext context)
    {
        if (playerStats.ToolMode == 0)
            playerStats.ToolMode = PlayerStats.ChipToolMode.Out;
        else
            PlayerStats.instance.ToolMode--;
    }

    private void CollectorRightCallback(InputAction.CallbackContext context)
    {
        if (playerStats.ToolMode == PlayerStats.ChipToolMode.Out)
            playerStats.ToolMode = 0;
        else
            playerStats.ToolMode++;
    }

    private void JumpCallback(InputAction.CallbackContext context)
    {
        if(!floorTrigger.isEmpty)
            rb.AddForce(Vector3.up * 3, ForceMode.VelocityChange);
    }

    private void CameraScrollCallback(InputAction.CallbackContext context)
    {
        var pos = playerCamera.transform.localPosition;
        pos.z += playerControls.CameraScroll.ReadValue<float>() * Time.deltaTime;
        playerCamera.transform.localPosition = pos;
    }

    void FixedUpdate()
    {
        Vector2 move = playerControls.Walk.ReadValue<Vector2>();
        dir.x = move.x;
        dir.z = move.y;

        var newDir = cameraRotationHelperY.transform.rotation * dir;

        rb.AddForce(newDir * playerStats.Speed);
        rb.velocity = new Vector3(rb.velocity.x * 0.9f, rb.velocity.y, rb.velocity.z * 0.9f);

        if (move.sqrMagnitude >= Mathf.Epsilon)
            movementRotationHelper.transform.rotation = Quaternion.Slerp(movementRotationHelper.transform.rotation, 
                Quaternion.LookRotation(newDir), 0.15f * Time.deltaTime * 60);

        if(transform.position.y < -5)
        {
            var vel = rb.velocity;
            vel.y = 100.0f;
            rb.velocity = vel;
        }
        else if (transform.position.y < -0.1)
        {
            var vel = rb.velocity;
            vel.y = 7.5f;
            rb.velocity = vel;
        }

        if (transform.position.y > 30)
            rb.AddForce(Vector3.down * (transform.position.y * Time.deltaTime * 10));

        if (mapRange.bounds.Contains(transform.position))
        {
            playerCamera.clearFlags = CameraClearFlags.Skybox;
            globalLight.enabled = true;
        }
        else
        {
            playerCamera.clearFlags = CameraClearFlags.Nothing;
            globalLight.enabled = false;
        }

    }

    private void MouseChangeCallback(InputAction.CallbackContext context)
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 rotation = context.ReadValue<Vector2>();

        cameraRotationHelperY.transform.Rotate(Vector3.up * (rotation.x * Globals.mouseSensitivity));
        cameraRotationHelperX.transform.Rotate(Vector3.left * (rotation.y * Globals.mouseSensitivity));
    }
    public void RespawnPlayer()
    {
        transform.SetPositionAndRotation(defaultPos, defaultRot);
    }

    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
