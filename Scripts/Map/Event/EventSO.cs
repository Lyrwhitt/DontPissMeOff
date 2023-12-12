using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EventSO", menuName = "ScriptableObjects/Event Data", order = 1)]
public class EventSO : ScriptableObject
{
    public float timer;

    public GameObject enemySet;
}