using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHit : MonoBehaviour
{
    public GameObject playerHitImage;  // Unity 에디터에서 연결할 PlayerHitImage 변수
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

        // hit 벡터를 카메라 좌표계로 변환
        Vector3 hitInCameraSpace = Camera.main.WorldToScreenPoint(hit);

        // 원의 위치를 UI 좌표로 변환
        Vector2 circlePosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // 원의 위치와 화면 좌표를 이용하여 각도 계산
        float angle = Mathf.Atan2(hitInCameraSpace.y - circlePosition.y, hitInCameraSpace.x - circlePosition.x);

        // 각도를 기반으로 원 위의 지점 계산
        float offsetX = Mathf.Cos(angle) * distanceFromCrosshair;
        float offsetY = Mathf.Sin(angle) * distanceFromCrosshair;

        // 원 바깥쪽에 위치한 지점 계산
        Vector2 outerPoint = circlePosition + new Vector2(offsetX, offsetY);

        // UI 오브젝트의 위치를 조정
        playerHitImage.transform.position = outerPoint;
        playerHitImage.transform.rotation = Quaternion.Euler(0,0, angle * Mathf.Rad2Deg -90f);

        // UI 표시
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
        // 화면 중앙을 기준으로 반지름이 circleRadius인 원의 위치를 계산
        Vector2 circlePosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // 원 바깥쪽에 위치한 지점 계산
        Vector2 outerPoint = circlePosition + (Vector2.up * distanceFromCrosshair);

        // 원의 위치를 UI 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, outerPoint, null, out Vector2 localPoint);

        // 이미지의 위치를 조정하여 원 바깥에 표시
        playerHitImage.transform.localPosition = localPoint;
        playerHitImage.transform.rotation = Quaternion.identity;
    }

    IEnumerator DeactivateEffectAfterDelay()
    {
        yield return delayTime;

        // 이미지를 비활성화하여 피격 이펙트를 숨김
        playerHitImage.SetActive(false);
        deactivateCoroutine = null; // 코루틴이 종료되었음을 나타내는 변수 초기화
    }
}
