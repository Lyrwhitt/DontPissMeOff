using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.isRunning = true;
        stateMachine.movementSpeedModifier = groundData.runSpeedModifier;
        base.Enter();

        SetAnimationBool(stateMachine.player.animationData.runParameterHash, true);
    }

    public override void Exit()
    {
        stateMachine.isRunning = false;

        base.Exit();

        SetAnimationBool(stateMachine.player.animationData.runParameterHash, false);
    }

    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        base.OnRunCanceled(context);
        stateMachine.ChangeState(stateMachine.walkState);
    }
}
