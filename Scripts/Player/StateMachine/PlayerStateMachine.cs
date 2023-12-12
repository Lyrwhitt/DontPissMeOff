using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;


public class PlayerStateMachine : StateMachine
{
    public Player player;

    public bool isRunning = false;

    [Header("State")]
    public PlayerIdleState idleState;
    public PlayerWalkState walkState;
    public PlayerRunState runState;
    public PlayerEvadeState evadeState;
    public PlayerEventState eventState;
    public PlayerDeathState deathState;

    [Header("Movement")]
    public float animationBlend = 0f;
    public Vector2 movementInput;
    public float movementSpeed;
    public float rotationDamping;
    public float movementSpeedModifier = 1f;

    [Header("Jump")]
    public float jumpForce;


    public Transform mainCameraTransform;

    
    public PlayerStateMachine(Player player)
    {
        this.player = player;

        idleState = new PlayerIdleState(this);
        walkState = new PlayerWalkState(this);
        runState = new PlayerRunState(this);
        evadeState = new PlayerEvadeState(this);
        eventState = new PlayerEventState(this);
        deathState = new PlayerDeathState(this);

        mainCameraTransform = Camera.main.transform;

        movementSpeed = player.data.groundedData.baseSpeed;
    }
    
}

