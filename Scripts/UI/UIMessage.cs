using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    private float _showTimer = 2.0f;
    private float _disappearTimer = 0.7f;

    float _accumTime = 0f;

    public TextMeshProUGUI message;

    public void ShowText(string text)
    {
        message.text = text;

        StartCoroutine(ShowTextCoroutine());
    }

    private IEnumerator ShowTextCoroutine()
    {
        Vector3 originPos = this.transform.position;

        while (true)
        {
            if (_showTimer <= 0f)
                break;

            _showTimer -= Time.deltaTime;

            yield return null;
        }

        while (_accumTime < _disappearTimer)
        {
            message.alpha = Mathf.Lerp(1f, 0f, _accumTime / _disappearTimer);

            yield return null;

            _accumTime += Time.deltaTime;
        }

        message.alpha = 0f;

        // objectPool 사용시 변경
        Destroy(this.gameObject);
    }
}