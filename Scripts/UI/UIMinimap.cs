using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIMinimap : MonoBehaviour
{
    public RectTransform map2d;
    public Transform player;
    public float minX = -140f;
    public float maxX = 140f;
    public float minY = -130f;
    public float maxY = 130f;
    public float moveXSpeed = 3.8f;
    public float moveYSpeed = 4.5f;

    private Vector3 playerLastPosition;

    private void Start()
    {
        if (player != null)
        {
            // 초기화 시 플레이어의 초기 위치 저장
            playerLastPosition = player.position;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            // 플레이어의 현재 위치를 가져옵니다.
            Vector3 playerPosition = player.position;

            // 플레이어가 이동한 만큼 맵의 위치를 반대로 이동시킵니다.
            Vector3 offset = playerPosition - playerLastPosition;

            // 플레이어의 z방향 이동을 미니맵의 x 방향 이동으로 변환합니다.
            float mapMovementX = -offset.z * moveXSpeed;

            // 플레이어의 x방향 이동을 미니맵의 y 방향 이동으로 변환합니다.
            float mapMovementY = offset.x * moveYSpeed;


            // 미니맵의 움직임을 플레이어와 동기화하되, 특정 범위 내에서만 움직이도록 제한합니다.
            float clampedMapX = Mathf.Clamp(map2d.localPosition.x + mapMovementX, minX, maxX);
            float clampedMapY = Mathf.Clamp(map2d.localPosition.y + mapMovementY, minY, maxY);

            // 미니맵의 위치를 업데이트합니다.
            map2d.localPosition = new Vector3(clampedMapX, clampedMapY, 0);

            // 플레이어의 현재 위치를 저장합니다.
            playerLastPosition = playerPosition;
        }
    }
}