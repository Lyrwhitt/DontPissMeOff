using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRandomWalkState : EnemyBaseState
{
    public EnemyRandomWalkState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0.4f;

        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Enemy.AnimationData.WalkParameterHash);
        SetRandomDestination();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Enemy.AnimationData.WalkParameterHash);
    }

    public override void Update()
    {
         float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Walk");
        if (normalizedTime <= 1f)
        {
            // NavMeshAgent가 목적지에 도달하면 대기
            if (!navMesh.pathPending && navMesh.remainingDistance < 0.1f)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }
    }
    void SetRandomDestination()
    {
        // 랜덤한 방향 생성
        Vector2 randomDirection = Random.insideUnitCircle.normalized * stateMachine.Enemy.Data.detectionRange * 0.8f;

        // 현재 위치에 방향을 더하여 랜덤한 위치 얻기
        Vector3 randomPosition = new Vector3(randomDirection.x, 0f, randomDirection.y) + stateMachine.Enemy.originalPosition.position;

        // NavMesh 위의 가장 가까운 위치 찾기
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, stateMachine.Enemy.Data.detectionRange, NavMesh.AllAreas))
        {
            // NavMeshAgent의 목적지 설정
            navMesh.SetDestination(hit.position);
        }
    }
}
