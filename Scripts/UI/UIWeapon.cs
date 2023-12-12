using UnityEngine;
using UnityEngine.UI;

public class UIWeapon : MonoBehaviour
{
    public GunType gunType;

    public Image iconImg;
    public Image toggleImg;

    public Sprite toggleOnSprite;
    public Sprite toggleOffSprite;

    [HideInInspector]
    public Toggle toggle;
    [HideInInspector]
    public bool isUnlocked = false;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        // ������ ������ ������ �ѵ��� ��� ���·� ����
        if (gunType != GunType.PISTOL)
        {
            isUnlocked = false;
            iconImg.enabled = false; // UI �̹��� ��Ȱ��ȭ
        }
        else
        {
            isUnlocked = true; // ������ �⺻������ ��� ���� ����
        }
    }
}
