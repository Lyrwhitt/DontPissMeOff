using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEventState : PlayerGroundState
{
    public PlayerEventState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        if (stateMachine.player.currentWeapon.isScoped)
            stateMachine.player.currentWeapon.OnUnScope();

        stateMachine.player.isEventMode = true;
        stateMachine.movementSpeedModifier = 0f;
        base.Enter();
        SetAnimationBool(stateMachine.player.animationData.idleParameterHash, true);

        CameraManager.Instance.ChangeCameraView(stateMachine.player.currentEvent.shotCamera);
        CameraManager.Instance.ChangeBlendTime(0f);
        CameraManager.Instance.ChangeBlendStyle(CinemachineBlendDefinition.Style.HardOut);

        stateMachine.player.transform.position = new Vector3(stateMachine.player.currentEvent.shotCamera.transform.position.x, stateMachine.player.transform.position.y,
    stateMachine.player.currentEvent.shotCamera.transform.position.z);

        //Shot();

    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hide();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Shot();
        }
    }

    public override void Exit()
    {
        if (stateMachine.player.currentWeapon.isScoped)
            stateMachine.player.currentWeapon.OnUnScope();

        stateMachine.player.isEventMode = false;

        stateMachine.player.isHide = false;
        stateMachine.player.status.isinvincible = false;

        stateMachine.player.ChangeCameraView(false);

        base.Exit();

        SetAnimationBool(stateMachine.player.animationData.idleParameterHash, false);
    }

    private void Hide()
    {
        if (stateMachine.player.currentWeapon.isScoped)
            stateMachine.player.currentWeapon.OnUnScope();

        CameraManager.Instance.ChangeCameraView(stateMachine.player.currentEvent.hideCamera);
        CameraManager.Instance.ChangeBlendTime(0.5f);
        stateMachine.player.transform.position = new Vector3(stateMachine.player.currentEvent.hideCamera.transform.position.x, stateMachine.player.transform.position.y,
            stateMachine.player.currentEvent.hideCamera.transform.position.z);
        stateMachine.player.isHide = true;
        stateMachine.player.status.isinvincible = true;
    }

    private void Shot()
    {
        CameraManager.Instance.ChangeCameraView(stateMachine.player.currentEvent.shotCamera);
        CameraManager.Instance.ChangeBlendTime(0.2f);
        stateMachine.player.transform.position = new Vector3(stateMachine.player.currentEvent.shotCamera.transform.position.x, stateMachine.player.transform.position.y,
            stateMachine.player.currentEvent.shotCamera.transform.position.z);
        stateMachine.player.isHide = false;
        stateMachine.player.status.isinvincible = false;
    }
}
