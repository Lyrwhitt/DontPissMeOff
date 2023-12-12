using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetPositionData", menuName = "Custom/TargetPositionData", order = 1)]
public class TargetPositionData : ScriptableObject
{
    // targetPositions -> enemy
    public List<Vector3> targetPositions = new List<Vector3>();
}
