using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGroundData
{
    [field: SerializeField][field: Range(0f, 25f)] public float baseSpeed { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 25f)] public float baseRotationDamping { get; private set; } = 1f;
    [field: SerializeField][field: Range(0f, 25f)] public float speedChangeRate { get; private set; } = 10f;

    [field: SerializeField][field: Range(0f, 2f)] public float walkSpeedModifier { get; private set; } = 0.225f;
    [field: SerializeField][field: Range(0f, 2f)] public float runSpeedModifier { get; private set; } = 1f;


    [field: SerializeField][field: Range(0f, 25f)] public float evadeStrength { get; private set; } = 5f;
    //[field: Header("Mouse")]
    //[field: SerializeField][field: Range(0f, 100f)] public float rotCamAxis
}

