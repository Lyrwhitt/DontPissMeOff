using System;
using UnityEngine;

[Serializable]
public class BossAnimationData
{
    [SerializeField] private string _chaseParameterName = "Chase";
    [SerializeField] private string _isDieParameterName = "IsDie";
    [SerializeField] private string _jumpParameterName = "Jump";

    [SerializeField] private string _attackShoulderMissileParameterName = "AttackShoulderMissile";
    [SerializeField] private string _attackLeftArmBeamParameterName = "AttackLeftArmBeam";
    [SerializeField] private string _attackCannonParameterName = "AttackCannon";
    [SerializeField] private string _attackStompParameterName = "AttackStomp";

    [SerializeField] private string _dirZParameterName = "DirZ";

    [SerializeField] private string _destroyedLeftArm = "DestroyedLeftArm";
    [SerializeField] private string _destroyedRightArm = "DestroyedRightArm";

    public int ChaseParameterHash { get; private set; }
    public int IsDieParameterHash { get; private set; }
    public int JumpParameterHash { get; private set; }

    public int AttackShoulderMissileParameterHash { get; private set; }
    public int AttackLeftArmBeamParameterHash { get; private set; }
    public int AttackRightArmCannonParameterHash { get; private set; }
    public int AttackStompParameterHash { get; private set; }

    public int DirZParameterHash { get; private set; }
    public int DestroyedLeftArmParameterHash { get; private set; }
    public int DestroyedRightArmParameterHash { get; private set; }


    public void Initialize()
    {
        ChaseParameterHash = Animator.StringToHash(_chaseParameterName);
        IsDieParameterHash = Animator.StringToHash(_isDieParameterName);
        JumpParameterHash = Animator.StringToHash(_jumpParameterName);

        AttackShoulderMissileParameterHash = Animator.StringToHash(_attackShoulderMissileParameterName);
        AttackLeftArmBeamParameterHash = Animator.StringToHash(_attackLeftArmBeamParameterName);
        AttackRightArmCannonParameterHash = Animator.StringToHash(_attackCannonParameterName);
        AttackStompParameterHash = Animator.StringToHash(_attackStompParameterName);

        DirZParameterHash = Animator.StringToHash(_dirZParameterName);
        DestroyedLeftArmParameterHash = Animator.StringToHash(_destroyedLeftArm);
        DestroyedRightArmParameterHash = Animator.StringToHash(_destroyedRightArm);
    }
}
