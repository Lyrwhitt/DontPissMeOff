using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        //ChangeCameraView(false);

        stateMachine.movementSpeedModifier = 0f;
        base.Enter();

        SetAnimationBool(stateMachine.player.animationData.idleParameterHash, true);
    }

    public override void Exit()
    {
        base.Exit();

        SetAnimationBool(stateMachine.player.animationData.idleParameterHash, false);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.movementInput != Vector2.zero)
        {
            OnMove();
            return;
        }
    }
}
