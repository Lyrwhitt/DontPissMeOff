using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGuide : MonoBehaviour
{
    public TextMeshProUGUI guideText;

    public void UpdateUI(GuideContent content)
    {
        guideText.SetText(content.content);
    }
}
