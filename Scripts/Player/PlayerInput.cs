using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputAction;
    public PlayerInputActions.PlayerActions playerActions;
    public PlayerInputActions.UIActions uiActions;

    private void Awake()
    {
        inputAction = new PlayerInputActions();
        playerActions = inputAction.Player;
        uiActions = inputAction.UI;
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }
}
