using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuideController : MonoBehaviour
{
    private bool isEnd = false;

    public UIGuide guideView;
    public Queue<GuideContent> guideContents = new Queue<GuideContent>();

    private void Start()
    {
        guideView.gameObject.SetActive(false);
    }

    public bool GuideEnded()
    {
        return isEnd;
    }

    private void SetGuideContent(GuideData guideData)
    {
        for(int i = 0; i < guideData.guideContents.Count; i++)
        {
            guideContents.Enqueue(guideData.guideContents[i]);
        }
    }

    public void StartGuide(GuideData guideData, bool withPause = true)
    {
        PlayerPrefs.SetString(guideData.name, "Complete");

        guideView.gameObject.SetActive(true);

        SetGuideContent(guideData);

        StartCoroutine(StartGuideCoroutine(withPause));
    }

    public IEnumerator StartGuideCoroutine(bool withPause)
    {
        WaitUntil waitUntil = new WaitUntil(() => isEnd);
        WaitForNextFrameUnit waitForNextFrame = new WaitForNextFrameUnit();

        yield return waitForNextFrame;

        if(withPause)
            GameManager.Instance.PauseGameForTutorial();

        while (guideContents.Count > 0)
        {
            isEnd = false;

            GuideContent content = guideContents.Dequeue();

            guideView.UpdateUI(content);

            if (content.waitInput.enabled)
            {
                StartCoroutine(WaitForInput(content.waitInput.inputKey, content.waitInput.waitAterInput));
            }
            else if (content.waitTime.enabled)
            {
                StartCoroutine(WaitForTime(content.waitTime.waitTime));
            }

            yield return waitUntil;
        }

        guideView.gameObject.SetActive(false);
        guideContents.Clear();

        if (withPause)
            GameManager.Instance.ResumeGame();
    }

    private IEnumerator WaitForInput(KeyCode keyCode, float afterWaitTime)
    {
        WaitUntil waitUntil = new WaitUntil(() => Input.GetKeyDown(keyCode));
        WaitForSecondsRealtime waitForSeconds = new WaitForSecondsRealtime(1.3f);

        yield return waitForSeconds;

        yield return waitUntil;

        waitForSeconds = new WaitForSecondsRealtime(afterWaitTime);

        yield return waitForSeconds;

        isEnd = true;
    }

    private IEnumerator WaitForTime(float waitTime)
    {
        WaitForSecondsRealtime waitForSeconds = new WaitForSecondsRealtime(waitTime);

        yield return waitForSeconds;

        isEnd = true;
    }
}
