using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : BaseState
{
    public PlayerGroundState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        SetAnimationBool(stateMachine.player.animationData.groundParameterHash, true);
    }

    public override void Exit()
    {
        base.Exit();

        SetAnimationBool(stateMachine.player.animationData.groundParameterHash, false);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.player.isEventMode)
            return;

        if (stateMachine.movementInput == Vector2.zero)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.idleState);

        base.OnMovementCanceled(context);
    }

    protected override void OnEvadeStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.player.status.isinvincible || stateMachine.player.isEventMode || GameManager.Instance.isPause)
            return;

        stateMachine.ChangeState(stateMachine.evadeState);

        base.OnEvadeStarted(context);
    }

    protected virtual void OnMove()
    {
        if (stateMachine.player.isEventMode)
            return;

        stateMachine.ChangeState(stateMachine.walkState);
    }

    protected virtual void OnEvade()
    {
        if (stateMachine.player.isEventMode)
            return;

        stateMachine.ChangeState(stateMachine.evadeState);
    }
}
