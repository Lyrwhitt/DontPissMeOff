using UnityEngine;

public class BossRobotIdleState : BossRobotBaseState
{
    private int _attactNumber = 0;
    private float _idleTime;

    public BossRobotIdleState(BossRobotStateMachine bossRobotStateMachine) : base(bossRobotStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StopNavMechAgent();
        _idleTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }
        else if (IsInStompAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackStompState);
            return;
        }
        else if(Time.time - _idleTime  > 2f)
        {
            Attack();
            return;
        }
    }

    private void Attack()
    {
        switch (_attactNumber)
        {
            case 0:
                stateMachine.ChangeState(stateMachine.AttackMissileState);
                break;
            case 1:
                if (stateMachine.BossRobot.IsRightArmHealthZero())
                    break;
                stateMachine.ChangeState(stateMachine.AttackCannonState);
                break;
            case 2:
                if (stateMachine.BossRobot.IsLeftArmHealthZero())
                    break;
                stateMachine.ChangeState(stateMachine.AttackBeamState);
                break;
            default:
                stateMachine.ChangeState(stateMachine.AttackMissileState);
                break;
        }
        _attactNumber += 1;
        if (_attactNumber == 3)
            _attactNumber = 0;
    }
}
