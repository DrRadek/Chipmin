using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject escMenu;
    [SerializeField] PlayerController playerController;

    private Controls controls;
    private Controls.UIActions uIControls;

    private bool escMenuOpen = false;

    private void Awake()
    {
        controls = new Controls();
        uIControls = controls.UI;
        uIControls.CursorSwitch.performed += CursorSwitchClicked;
        uIControls.MenuOpen.performed += OnEscClicked;
        Globals.lastCursorLockState = Cursor.lockState;
    }

    private void CursorSwitchClicked(InputAction.CallbackContext context)
    {
        Cursor.lockState = 1 - Cursor.lockState;
        Globals.lastCursorLockState = Cursor.lockState;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            Cursor.lockState = Globals.lastCursorLockState;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void OnEscClicked(InputAction.CallbackContext context)
    {
        escMenuOpen = !escMenuOpen;

        if (escMenuOpen)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;

        Globals.lastCursorLockState = Cursor.lockState;

        playerController.enabled = !escMenuOpen;
        escMenu.SetActive(escMenuOpen);
    }
}
