using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

public class UIDamage : MonoBehaviour
{
    public GameObject canvas;
    [SerializeField] private string damageTextPoolTag = "DamageText"; // 풀에서 가져오도록 변경
    [SerializeField] private float fadeDuration = 1.0f; // 텍스트가 사라지는 시간 설정

    private Dictionary<TextMeshProUGUI, Coroutine> fadeOutCoroutines;

    private void Start()
    {
        fadeOutCoroutines = new Dictionary<TextMeshProUGUI, Coroutine>();
    }

    public void ShowDamageText(int damage, Vector3 position, DamageType damagetype = DamageType.Body)
    {
        TextMeshProUGUI damageText = ObjectPool.Instance.SpawnFromPool(damageTextPoolTag, Vector3.zero, Quaternion.identity).GetComponent<TextMeshProUGUI>();
        damageText.transform.SetParent(canvas.transform);
        if (damageText != null)
        {
            damageText.text = damage.ToString();
            damageText.transform.position = Camera.main.WorldToScreenPoint(position);
            switch (damagetype)
            {
                case DamageType.Head:
                    damageText.color = Color.red;
                    break;
                case DamageType.Body:
                    damageText.color = Color.white;
                    break;
            }

            if (fadeOutCoroutines.ContainsKey(damageText) && fadeOutCoroutines[damageText] != null)
            {
                StopCoroutine(fadeOutCoroutines[damageText]);
                fadeOutCoroutines.Remove(damageText);
            }
            fadeOutCoroutines[damageText] = StartCoroutine(FadeOutText(damageText));
        }
    }

    IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color initialColor = text.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            text.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.text = "";
        text.color = initialColor;
        ObjectPool.Instance.ReturnToPool(damageTextPoolTag, text.gameObject);
    }
}
