using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsChangeType
{
    Melee,
    Rifle,
    Sniper,
    Sniper2,
    Elite,
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Custom/EnemyData", order = 1)]
public class EnemySpawnData : ScriptableObject
{
    public int sectorIdx;
    public StatsChangeType statsChangeType = StatsChangeType.Rifle;
    // targetPositions -> enemy
    public Vector3 position;
}
