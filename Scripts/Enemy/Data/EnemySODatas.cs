using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySODatas", menuName = "Characters/EnemyDatas")]

public class EnemySODatas : ScriptableObject
{
    [field: SerializeField] public EnemySO EnemyBaseSO { get; private set; }
    [field: SerializeField] public EnemySO EnemyMeleeSO { get; private set; }
    [field: SerializeField] public EnemySO EnemySniperSO { get; private set; }
    [field: SerializeField] public EnemySO EnemyEliteSO { get; private set; }
}
