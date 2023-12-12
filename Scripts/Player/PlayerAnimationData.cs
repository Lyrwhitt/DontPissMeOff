using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    public Dictionary<int, bool> boolParameters = new Dictionary<int, bool>();
    public Dictionary<int, float> floatParameters = new Dictionary<int, float>();

    [SerializeField] private string _groundParameterName = "@Ground";
    [SerializeField] private string _idleParameterName = "Idle";
    [SerializeField] private string _walkParameterName = "Walk";
    [SerializeField] private string _runParameterName = "Run";
    [SerializeField] private string _speedParameterName = "Speed";
    [SerializeField] private string _evadeParameterName = "Evade";
    [SerializeField] private string _changedParameterName = "Changed";
    [SerializeField] private string _deathParameterName = "Death";


    public int groundParameterHash { get; private set; }
    public int idleParameterHash { get; private set; }
    public int walkParameterHash { get; private set; }
    public int runParameterHash { get; private set; }
    public int speedParameterHash { get; private set; }
    public int evadeParameterHash { get; private set; }
    public int changedParameterHash { get; private set; }
    public int deathParameterHash { get; private set; }

    public void Initialize()
    {
        groundParameterHash = Animator.StringToHash(_groundParameterName);
        idleParameterHash = Animator.StringToHash(_idleParameterName);
        walkParameterHash = Animator.StringToHash(_walkParameterName);
        runParameterHash = Animator.StringToHash(_runParameterName);
        speedParameterHash = Animator.StringToHash(_speedParameterName);
        evadeParameterHash = Animator.StringToHash(_evadeParameterName);
        changedParameterHash = Animator.StringToHash(_changedParameterName);
        deathParameterHash = Animator.StringToHash(_deathParameterName);

        boolParameters.Add(groundParameterHash, false);
        boolParameters.Add(idleParameterHash, false);
        boolParameters.Add(walkParameterHash, false);
        boolParameters.Add(runParameterHash, false);
    }
}
