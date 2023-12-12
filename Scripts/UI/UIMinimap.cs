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
            // �ʱ�ȭ �� �÷��̾��� �ʱ� ��ġ ����
            playerLastPosition = player.position;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            // �÷��̾��� ���� ��ġ�� �����ɴϴ�.
            Vector3 playerPosition = player.position;

            // �÷��̾ �̵��� ��ŭ ���� ��ġ�� �ݴ�� �̵���ŵ�ϴ�.
            Vector3 offset = playerPosition - playerLastPosition;

            // �÷��̾��� z���� �̵��� �̴ϸ��� x ���� �̵����� ��ȯ�մϴ�.
            float mapMovementX = -offset.z * moveXSpeed;

            // �÷��̾��� x���� �̵��� �̴ϸ��� y ���� �̵����� ��ȯ�մϴ�.
            float mapMovementY = offset.x * moveYSpeed;


            // �̴ϸ��� �������� �÷��̾�� ����ȭ�ϵ�, Ư�� ���� �������� �����̵��� �����մϴ�.
            float clampedMapX = Mathf.Clamp(map2d.localPosition.x + mapMovementX, minX, maxX);
            float clampedMapY = Mathf.Clamp(map2d.localPosition.y + mapMovementY, minY, maxY);

            // �̴ϸ��� ��ġ�� ������Ʈ�մϴ�.
            map2d.localPosition = new Vector3(clampedMapX, clampedMapY, 0);

            // �÷��̾��� ���� ��ġ�� �����մϴ�.
            playerLastPosition = playerPosition;
        }
    }
}