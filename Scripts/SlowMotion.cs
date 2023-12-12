using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using System;

public class SlowMotion : MonoBehaviour
{
    private bool _isSlowing = false;

    public float lerpTime = 0.5f;
    public float volumeLerpTime = 0.5f;
    public float slowFactor = 0.05f;
    public float slowTime = 2f;

    public Action onEndSlowMotion;

    private float _timer = 0f; 

    [Header("Vignette")]

    private float _fixTimeScale = 1f;

    private void Start()
    {
        GameManager.Instance.onPause += OnPause;
        GameManager.Instance.onResume += OnResume;
    }

    public void StartSlowMotion()
    {
        if (_isSlowing)
            return;

        _isSlowing = true;

        StartCoroutine(SlowMotionCoroutine());
    }

    public IEnumerator SlowMotionCoroutine()
    {
        DOTween.To(() => 1f, timeScale => Time.timeScale = timeScale, slowFactor, lerpTime).SetEase(Ease.OutQuad).SetUpdate(true);

        _timer = slowTime;

        while (true)
        {
            if(_timer < 0f)
            {
                Time.timeScale = 1f;
                _isSlowing = false;

                break;
            }

            _timer -= Time.unscaledDeltaTime * _fixTimeScale;

            yield return null;
        }

        DOTween.To(() => slowFactor, timeScale => Time.timeScale = timeScale, 1f, lerpTime).SetEase(Ease.InQuad).SetUpdate(true);

        onEndSlowMotion?.Invoke();

        if (!PlayerPrefs.HasKey("SlowMotionGuide2"))
        {
            GameManager.Instance.guideController.StartGuide(Resources.Load<GuideData>("Guide/SlowMotionGuide2"), false);
        }

    }

    private void OnPause()
    {
        _fixTimeScale = 0f;
        _timer = 0f;
    }

    private void OnResume()
    {
        _fixTimeScale = 1f;
        _timer = 0f;
    }
}
