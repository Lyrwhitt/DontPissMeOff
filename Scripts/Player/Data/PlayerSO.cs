using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/PlayerData")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public PlayerGroundData groundedData { get; private set; }
    [field: SerializeField] public PlayerStatusData statusData { get; private set; }
}
