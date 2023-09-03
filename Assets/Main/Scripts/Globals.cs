using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Globals
{
    public static CursorLockMode lastCursorLockState;
    public static float mouseSensitivity = 0.1f;

    public static Color greenTransColor = new(0, 1, 0, 0.2f);
    public static Color redTransColor = new(1, 0, 0, 0.2f);
}
