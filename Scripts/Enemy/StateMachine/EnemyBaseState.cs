using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;

    protected readonly PlayerGroundData groundData;
    protected NavMeshAgent navMesh;
    protected Vector3 hitPoint;
    public EnemyBaseState(EnemyStateMachine ememyStateMachine)
    {
        stateMachine = ememyStateMachine;
        groundData = stateMachine.Enemy.Data.GroundedData;
        navMesh = stateMachine.Enemy.NavMeshAgent;
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
        navMesh.SetDestination(stateMachine.Target.transform.position);
        MoveTo(GetMovementDirection());
    }

    public virtual void FixedUpdate()
    {

    }
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, false);
    }

    protected void MoveTo(Vector3 movementDirection)
    {
        Rotate(movementDirection);
        Move(movementDirection);
    }
    protected void ForceMove()
    {
        stateMachine.Enemy.Controller.Move(stateMachine.Enemy.ForceReceiver.Movement * Time.deltaTime);
    }
    private Vector3 GetMovementDirection()
    {
        return navMesh.desiredVelocity.normalized;
    }

    protected void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Enemy.Controller.Move(((direction * movementSpeed) + stateMachine.Enemy.ForceReceiver.Movement) * Time.deltaTime);
    }
    protected void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            stateMachine.Enemy.transform.rotation = Quaternion.Slerp(stateMachine.Enemy.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;

        return movementSpeed;
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
    protected bool IsInChaseRange()
    {
        // if (stateMachine.Target.IsDead) { return false; }

        Vector3 enemyPosition = stateMachine.Enemy.transform.position + new Vector3(0, 1f, 0) + stateMachine.Enemy.transform.forward;
        float ChasingRange = 1f;
        if (RaycastToTag(enemyPosition, stateMachine.Enemy.transform.forward, ChasingRange, "Enemy"))
            return false;
        else
        {
            float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;

            return playerDistanceSqr <= stateMachine.Enemy.Data.PlayerChasingRange * stateMachine.Enemy.Data.PlayerChasingRange;
        }
    }
    protected bool IsInAttackRange()
    {
        // if (stateMachine.Target.IsDead) { return false; }

        Vector3 enemyPosition = stateMachine.Enemy.transform.position + new Vector3(0, 1f, 0);
        float attackRange = stateMachine.Enemy.Data.AttackRange;

        if (RaycastToTag(enemyPosition, stateMachine.Enemy.transform.forward, attackRange, "Player"))
        {
            float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;
            return playerDistanceSqr <= stateMachine.Enemy.Data.AttackRange * stateMachine.Enemy.Data.AttackRange;
        }

        return false;
    }

    private bool RaycastToTag(Vector3 origin, Vector3 direction, float distance, string targetTag)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        int layerMask = ~((1 << LayerMask.NameToLayer("Ignore")) | (1 << LayerMask.NameToLayer("Trajectory")));

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            if (!hit.collider.CompareTag(targetTag))
            {
                return false;
            }

            hitPoint = hit.point;
            return true;
        }
        return false;
    }
}