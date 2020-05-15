using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one InputManager in scene");
            return;
        }

        instance = this;

    }

    public KeyCode pauseGame;
    public KeyCode forward;
    public KeyCode left;
    public KeyCode backward;
    public KeyCode right;
    public KeyCode rotation;
    public KeyCode deselect;
}
