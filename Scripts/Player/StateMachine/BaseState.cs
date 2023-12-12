using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public BaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundData = stateMachine.player.data.groundedData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        if (stateMachine.player.status.isinvincible || GameManager.Instance.isPause)
            return;

        ReadMovementInput();
    }

    public virtual void Update()
    {
        Move();
        Rotate();
    }

    public virtual void FixedUpdate()
    {

    }


    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Movement.canceled += OnMovementCanceled;

        input.playerActions.Run.started += OnRunStarted;
        input.playerActions.Run.canceled += OnRunCanceled;

        input.playerActions.Evade.started += OnEvadeStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.player.input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;

        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Run.canceled -= OnRunCanceled;

        input.playerActions.Evade.started -= OnEvadeStarted;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnEvadeStarted(InputAction.CallbackContext context)
    {

    }

    private void ReadMovementInput()
    {
        stateMachine.movementInput = stateMachine.player.input.playerActions.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        //Rotate(movementDirection);
        Move(movementDirection);
    }

    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovemenetSpeed();

        stateMachine.player.controller.Move(((movementDirection * movementSpeed) + stateMachine.player.forceReceiver.Movement) * Time.deltaTime);
    }

    private void Rotate()
    {
        stateMachine.player.thirdPerson.transform.rotation = Quaternion.Euler(0f, stateMachine.mainCameraTransform.eulerAngles.y, 0f);
    }

    protected Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.mainCameraTransform.forward;
        Vector3 right = stateMachine.mainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.movementInput.y + right * stateMachine.movementInput.x;
    }

    private float GetMovemenetSpeed()
    {
        float movementSpeed = stateMachine.movementSpeed * stateMachine.movementSpeedModifier;
        return movementSpeed;
    }


    private void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            Transform playerTransform = stateMachine.player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.rotationDamping * Time.deltaTime);
        }
    }

    protected void SetAnimationBool(int animationHash, bool value)
    {
        stateMachine.player.animator.SetBool(animationHash, value);
        stateMachine.player.animationData.boolParameters[animationHash] = value;
    }

    protected void SetAnimationFloat(int animationHash, float value)
    {
        stateMachine.player.animator.SetFloat(animationHash, value);            
    }

    protected void AnimationTrigger(int animationHash)
    {
        stateMachine.player.animator.SetTrigger(animationHash);
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
