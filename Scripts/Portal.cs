using Cinemachine;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform spawnpoint;
    public GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.SaveLastData();
            if (boss != null)
                boss.SetActive(true);

            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false; // ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
                other.transform.position = spawnpoint.position; // �÷��̾� ��ġ ����
                CinemachinePOV pov = GameManager.Instance.player.firstCamera.GetCinemachineComponent<CinemachinePOV>();
                pov.m_VerticalAxis.Value = spawnpoint.transform.rotation.eulerAngles.x;
                pov.m_HorizontalAxis.Value = spawnpoint.transform.rotation.eulerAngles.y;
                controller.enabled = true; // ĳ���� ��Ʈ�ѷ� Ȱ��ȭ
            }
        }
    }
}
