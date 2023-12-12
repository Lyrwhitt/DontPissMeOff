using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponAnimationData
{
    [SerializeField] private string _fireParameterName = "Fire";
    [SerializeField] private string _reloadParameterName = "Reload";
    [SerializeField] private string _reloadStartParameterName = "ReloadStart";
    [SerializeField] private string _reloadingParameterName = "Reloading";
    [SerializeField] private string _reloadEndParameterName = "ReloadEnd";
    [SerializeField] private string _throwParameterName = "Throw";

    public int fireParameterHash { get; private set; }
    public int reloadParameterHash { get; private set; }
    public int reloadStartParameterHash { get; private set; }
    public int reloadingParameterHash { get; private set; }
    public int reloadEndParameterHash { get; private set; }
    public int throwParameterHash { get; private set; }

    public void Initialize()
    {
        fireParameterHash = Animator.StringToHash(_fireParameterName);
        reloadParameterHash = Animator.StringToHash(_reloadParameterName);
        reloadStartParameterHash = Animator.StringToHash(_reloadStartParameterName);
        reloadingParameterHash = Animator.StringToHash(_reloadingParameterName);
        reloadEndParameterHash = Animator.StringToHash(_reloadEndParameterName);
        throwParameterHash = Animator.StringToHash(_throwParameterName);
    }
}
