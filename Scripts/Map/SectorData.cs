using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public enum SectorType
{
    Normal, Boss
}

// 제거 할것
public class SectorData : MonoBehaviour
{
    public SectorType sectorType;

    public Vector3 playerSpawnPos;
    public Vector3 playerSpawnRot;

    public List<GameObject> sectorWalls = new List<GameObject>();
    public List<GameObject> sectorItems = new List<GameObject>();

    public bool hasEvent = false;

    [HideInInspector] public List<EventScenario> eventScenario;

}