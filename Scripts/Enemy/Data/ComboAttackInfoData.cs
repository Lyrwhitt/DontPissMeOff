using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComboAttackInfoData
{
    [field: SerializeField] public string AttackName { get; private set; }
    [field: SerializeField] public string AttackSound { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; }
}


[Serializable]
public class EnemyAttackData
{
    [field: SerializeField] public List<ComboAttackInfoData> ComboAttackInfoDatas { get; private set; }
    public int GetAttackInfoCount() { return ComboAttackInfoDatas.Count; }
    public ComboAttackInfoData GetAttackInfo(int index) { return ComboAttackInfoDatas[index]; }
}
