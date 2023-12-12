using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Head, Body
}

public class EnemyCollider : MonoBehaviour
{
    [HideInInspector]
    public Health health;

    private void Awake()
    {
        health = this.transform.parent.parent.GetComponent<Health>();
    }

    public DamageType damageType;
}
