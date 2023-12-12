using UnityEngine;

public class BossRobotStateMachine : StateMachine
{
    public BossRobot BossRobot { get; }
    public Transform Player { get; private set; }

    public BossRobotIdleState IdleState { get; }
    public BossRobotChasingState ChasingState { get; }
    public BossRobotAttackMissileState AttackMissileState { get; }
    public BossRobotAttackBeamState AttackBeamState { get; }
    public BossRobotAttackCannonState AttackCannonState { get; }
    public BossRobotAttackStompState AttackStompState { get; }
    public BossRobotDestroyedLeftArmState DestroyedLeftArmState { get; }
    public BossRobotDestroyedRightArmState DestroyedRightArmState { get; }
    public BossRobotRepairState RepairState { get; }


    public float RotationDamping { get; private set; }

    public BossRobotStateMachine(BossRobot bossRobot)
    {
        BossRobot = bossRobot;
        Player = BossRobot.playerTransform;

        IdleState = new BossRobotIdleState(this);
        ChasingState = new BossRobotChasingState(this);

        AttackMissileState = new BossRobotAttackMissileState(this);
        AttackBeamState = new BossRobotAttackBeamState(this);
        AttackCannonState = new BossRobotAttackCannonState(this);
        AttackStompState = new BossRobotAttackStompState(this);

        DestroyedLeftArmState = new BossRobotDestroyedLeftArmState(this);
        DestroyedRightArmState = new BossRobotDestroyedRightArmState(this);

        RepairState = new BossRobotRepairState(this);
    }
}
