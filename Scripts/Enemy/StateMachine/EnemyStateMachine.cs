using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; }
    public Transform Target { get; private set; }

    public EnemyIdleState IdlingState { get; }
    public EnemyChasingState ChasingState { get; }
    public EnemyRifleAttackState RifleAttackState { get; }
    public EnemyCrouchIdleState CrouchIdleState { get; }
    public EnemyCrouchAttackState CrouchAttackState { get; }
    public EnemyComboAttackState ComboAttackState { get; }
    public EnemyMovePosState MovePosState { get; }
    public EnemyRandomWalkState RandowWalkState { get; }
    public EnemyProneAttackState ProneAttackState { get; }
    public EnemyWalkCrouchingAttackState WalkCrouchingAttackState { get; }

    public int ComboIndex { get; set; }
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public EnemyStateMachine(Enemy enemy)
    {
        Enemy = enemy;
        Target = GameObject.FindGameObjectWithTag("Player").transform;

        IdlingState = new EnemyIdleState(this);
        ChasingState = new EnemyChasingState(this);
        RifleAttackState = new EnemyRifleAttackState(this);
        MovePosState = new EnemyMovePosState(this);
        RandowWalkState = new EnemyRandomWalkState(this);
        CrouchIdleState = new EnemyCrouchIdleState(this);
        CrouchAttackState = new EnemyCrouchAttackState(this);
        ComboAttackState = new EnemyComboAttackState(this);
        ProneAttackState = new EnemyProneAttackState(this);
        WalkCrouchingAttackState = new EnemyWalkCrouchingAttackState(this);

        MovementSpeed = enemy.Data.GroundedData.baseSpeed;
        RotationDamping = enemy.Data.GroundedData.baseRotationDamping;
    }
}
