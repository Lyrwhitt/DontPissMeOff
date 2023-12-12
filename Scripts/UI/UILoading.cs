using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class UILoading : MonoBehaviour
{
    public bool isStart = false;
    public bool isFinish = false;

    public Slider slider;
    public Text progressText;

    public float speed = 0.5f;

    float time = 0f;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void StartLoading()
    {
        if (isStart)
            return;

        StartCoroutine(LoadingCoroutine());
    }

    public void UpdateProgress(float content)
    {
        progressText.text = Mathf.Round(content * 100) + "%";
    }

    public IEnumerator LoadingCoroutine()
    {
        isStart = true;

        while (true)
        {
            time += Time.deltaTime * speed;
            slider.value = time;

            UpdateProgress(time);

            if (time > 1)
            {
                isStart = false;
                isFinish = true;
                time = 0;

                break;
            }

            yield return null;
        }
    }
}
