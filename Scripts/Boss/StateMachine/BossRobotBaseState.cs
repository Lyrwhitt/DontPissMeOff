using UnityEngine;
using UnityEngine.AI;

public class BossRobotBaseState : IState
{
    protected BossRobotStateMachine stateMachine;

    protected Animator animator;
    protected NavMeshAgent navMeshAgent;

    protected Vector3 playerPos;
    protected Vector3 playerGroundPos;
    protected Vector3 bossRobotPos;

    protected float minAttackDistance;
    protected float maxAttackDistance;
    protected float attackHeight;
    protected float lastAttackTime = 0f;


    private float _minAttactAngle;
    private float _maxAttactAngle;
    private bool _isRotate = true;

    public BossRobotBaseState(BossRobotStateMachine bossRobotStateMachine)
    {
        stateMachine = bossRobotStateMachine;

        animator = stateMachine.BossRobot.GetAnimator();
        navMeshAgent = stateMachine.BossRobot.GetNavMeshAgent();

        minAttackDistance = stateMachine.BossRobot.GetMinAttackDistance();
        maxAttackDistance = stateMachine.BossRobot.GetMaxAttackDistance();
        attackHeight = stateMachine.BossRobot.GetAttactHeight();
        _minAttactAngle = Mathf.Atan(minAttackDistance / attackHeight);
        _maxAttactAngle = Mathf.Atan(maxAttackDistance / attackHeight);
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void Update()
    {
        UpdatePosition();
        UpdateDirZ();

        navMeshAgent.SetDestination(playerPos);

        if (stateMachine.BossRobot.IsDead())
            _isRotate = false;

        if (!animator.GetBool(stateMachine.BossRobot.AnimationData.DestroyedLeftArmParameterHash) && stateMachine.BossRobot.IsLeftArmHealthZero())
            DestroyLeftArm();

        if (!animator.GetBool(stateMachine.BossRobot.AnimationData.DestroyedRightArmParameterHash) && stateMachine.BossRobot.IsRightArmHealthZero())
            DestroyRightArm();

        if (stateMachine.BossRobot.IsTimeToRepair())
            Repair();

        if (_isRotate)
            Rotate();
    }

    public virtual void FixedUpdate()
    {

    }

    private void Rotate()
    {
        stateMachine.BossRobot.transform.LookAt(playerGroundPos);
    }

    private void UpdatePosition()
    {
        playerPos = stateMachine.Player.position;
        playerGroundPos = playerPos;
        playerGroundPos.y = 0;
        bossRobotPos = stateMachine.BossRobot.transform.position;
    }

    private void UpdateDirZ()
    {
        float playerDistance = Vector3.Distance(playerGroundPos, bossRobotPos);
        float angle = Mathf.Atan(playerDistance / attackHeight);
        float dirZ = 0f;

        if ((_maxAttactAngle - _minAttactAngle) != 0)
            dirZ = (angle - _minAttactAngle) / (_maxAttactAngle - _minAttactAngle);

        animator.SetFloat(stateMachine.BossRobot.AnimationData.DirZParameterHash, dirZ);
    }

    private void Repair()
    {
        _isRotate = false;
        stateMachine.ChangeState(stateMachine.RepairState);
    }

    private void DestroyLeftArm()
    {
        _isRotate = false;
        stateMachine.ChangeState(stateMachine.DestroyedLeftArmState);
    }

    private void DestroyRightArm()
    {
        _isRotate = false;
        stateMachine.ChangeState(stateMachine.DestroyedRightArmState);
    }

    protected void StartAnimation(int animationHash)
    {
        animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        animator.SetBool(animationHash, false);
    }

    protected void TriggerAnimation(int animationHash)
    {
        animator.SetTrigger(animationHash);
    }

    protected void ResetTriggerAnimation(int animatinHash)
    {
        animator.ResetTrigger(animatinHash);
    }

    protected void StartNavMeshAgent()
    {
        navMeshAgent.isStopped = false;
        _isRotate = false;
    }

    protected void StopNavMechAgent()
    {
        navMeshAgent.isStopped = true;
        _isRotate = true;
    }

    protected bool IsInChaseRange()
    {
        float playerDistanceSqr = (playerGroundPos - bossRobotPos).sqrMagnitude;

        return playerDistanceSqr > maxAttackDistance * maxAttackDistance;
    }

    protected bool IsInStompAttackRange()
    {
        float playerDistanceSqr = (playerGroundPos - bossRobotPos).sqrMagnitude;

        return playerDistanceSqr <= minAttackDistance * minAttackDistance;
    }
}
