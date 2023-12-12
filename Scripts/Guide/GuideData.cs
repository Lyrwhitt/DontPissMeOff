using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "GuideData", menuName = "ScriptableObjects/Guide Data", order = 1)]
public class GuideData : ScriptableObject
{
    public List<GuideContent> guideContents;
}

[Serializable]
public class GuideContent
{
    [TextArea(4, 10)]
    public string content;

    public WaitInput waitInput = new WaitInput();

    public WaitTime waitTime = new WaitTime();
}

[Serializable, Toggle("enabled")]
public class WaitInput
{
    public bool enabled;
    public KeyCode inputKey;
    public float waitAterInput;
}

[Serializable, Toggle("enabled")]
public class WaitTime
{
    public bool enabled;
    public float waitTime;
}