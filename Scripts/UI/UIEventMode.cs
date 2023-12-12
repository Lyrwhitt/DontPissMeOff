using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEventMode : MonoBehaviour
{
    public TextMeshProUGUI timer;

    public void ChangeTimerValue(float time)
    {
        timer.text = time.ToString("F2");
    }

    public void InitEventUI()
    {
        timer.text = "0";
    }
}
