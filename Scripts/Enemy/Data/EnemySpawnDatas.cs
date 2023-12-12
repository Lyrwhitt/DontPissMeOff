using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnDatas", menuName = "Custom/EnemySpawnDatas", order = 2)]
public class EnemySpawnDatas : ScriptableObject
{
    public List<EnemySpawnData> spawnDatas = new List<EnemySpawnData>();
}
