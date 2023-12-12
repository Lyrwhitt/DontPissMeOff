using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAnimationData
{
    [SerializeField] private string _groundParameterName = "@Ground";
    [SerializeField] private string _idleParameterName = "Idle";
    [SerializeField] private string _walkParameterHash = "Walk";
    [SerializeField] private string _runParameterName = "Run";
    [SerializeField] private string _standParameterName = "Stand";

    [SerializeField] private string _crouchParameterName = "Crouch";

    [SerializeField] private string _attackParameterName = "@Attack";
    [SerializeField] private string _baseAttackParameterName = "BaseAttack";
    [SerializeField] private string _proneAttackParameterName = "ProneAttack";
    [SerializeField] private string _crouchingAttackParameterName = "CrouchingAttack";

    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }

    public int StandParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int BaseAttackParameterHash { get; private set; }
    public int CrouchParameterHash { get; private set; }
    public int ProneAttackParameterHash { get; private set; }
    public int CrouchingAttackParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(_groundParameterName);
        IdleParameterHash = Animator.StringToHash(_idleParameterName);
        WalkParameterHash = Animator.StringToHash(_walkParameterHash);
        RunParameterHash = Animator.StringToHash(_runParameterName);
        StandParameterHash = Animator.StringToHash(_standParameterName);
        AttackParameterHash = Animator.StringToHash(_attackParameterName);
        BaseAttackParameterHash = Animator.StringToHash(_baseAttackParameterName);
        
    }
    public void SniperInit()
    {
        CrouchParameterHash = Animator.StringToHash(_crouchParameterName);
    }
    public void EliteInit()
    {
        ProneAttackParameterHash = Animator.StringToHash(_proneAttackParameterName);
        CrouchingAttackParameterHash = Animator.StringToHash(_crouchingAttackParameterName);
    }
}
