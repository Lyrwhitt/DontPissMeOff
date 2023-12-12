using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EventTrigger : MonoBehaviour
{
    public EventScenario eventScenario;

    public GameObject cutScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CutSceneStart();
            //Play();
            /*
            eventScenario.StartEventMode();
            this.gameObject.SetActive(false);
            */
        }
    }

    public void CutSceneStart()
    {
        GameManager.Instance.player.ChangeCameraView(false);
        GameManager.Instance.playerSet.SetActive(false);

        cutScene.SetActive(true);
        GameManager.Instance.cutsceneCam.SetActive(true);
    }

    public void OnTimeLineEnd()
    {
        //GameManager.Instance.player.firstPerson.SetActive(true);
        GameManager.Instance.playerSet.SetActive(true);

        cutScene.SetActive(false);
        GameManager.Instance.cutsceneCam.SetActive(false);

        eventScenario.StartEventMode();
        this.gameObject.SetActive(false);
    }
}
