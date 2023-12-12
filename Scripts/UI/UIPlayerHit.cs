using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHit : MonoBehaviour
{
    public GameObject playerHitImage;  // Unity �����Ϳ��� ������ PlayerHitImage ����
    public float distanceFromCrosshair = 120f;

    private Coroutine deactivateCoroutine;
    private WaitForSeconds delayTime = new WaitForSeconds(1f);

    public void ShowHitEffect(Vector3 hit)
    {
        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
        }

        deactivateCoroutine = StartCoroutine(DeactivateEffectAfterDelay());

        // hit ���͸� ī�޶� ��ǥ��� ��ȯ
        Vector3 hitInCameraSpace = Camera.main.WorldToScreenPoint(hit);

        // ���� ��ġ�� UI ��ǥ�� ��ȯ
        Vector2 circlePosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // ���� ��ġ�� ȭ�� ��ǥ�� �̿��Ͽ� ���� ���
        float angle = Mathf.Atan2(hitInCameraSpace.y - circlePosition.y, hitInCameraSpace.x - circlePosition.x);

        // ������ ������� �� ���� ���� ���
        float offsetX = Mathf.Cos(angle) * distanceFromCrosshair;
        float offsetY = Mathf.Sin(angle) * distanceFromCrosshair;

        // �� �ٱ��ʿ� ��ġ�� ���� ���
        Vector2 outerPoint = circlePosition + new Vector2(offsetX, offsetY);

        // UI ������Ʈ�� ��ġ�� ����
        playerHitImage.transform.position = outerPoint;
        playerHitImage.transform.rotation = Quaternion.Euler(0,0, angle * Mathf.Rad2Deg -90f);

        // UI ǥ��
        playerHitImage.SetActive(true);
    }
    public void ShowHitEffect()
    {
        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
        }

        deactivateCoroutine = StartCoroutine(DeactivateEffectAfterDelay());
        playerHitImage.SetActive(true);
        // ȭ�� �߾��� �������� �������� circleRadius�� ���� ��ġ�� ���
        Vector2 circlePosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // �� �ٱ��ʿ� ��ġ�� ���� ���
        Vector2 outerPoint = circlePosition + (Vector2.up * distanceFromCrosshair);

        // ���� ��ġ�� UI ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, outerPoint, null, out Vector2 localPoint);

        // �̹����� ��ġ�� �����Ͽ� �� �ٱ��� ǥ��
        playerHitImage.transform.localPosition = localPoint;
        playerHitImage.transform.rotation = Quaternion.identity;
    }

    IEnumerator DeactivateEffectAfterDelay()
    {
        yield return delayTime;

        // �̹����� ��Ȱ��ȭ�Ͽ� �ǰ� ����Ʈ�� ����
        playerHitImage.SetActive(false);
        deactivateCoroutine = null; // �ڷ�ƾ�� ����Ǿ����� ��Ÿ���� ���� �ʱ�ȭ
    }
}
