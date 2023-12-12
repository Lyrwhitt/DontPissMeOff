using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStatusData
{
    [field: SerializeField] public int maxHealth = 100;

    [field: SerializeField] public float maxStamina = 100f;
}
