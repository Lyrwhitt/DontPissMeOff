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

        // 권총을 제외한 나머지 총들은 잠금 상태로 시작
        if (gunType != GunType.PISTOL)
        {
            isUnlocked = false;
            iconImg.enabled = false; // UI 이미지 비활성화
        }
        else
        {
            isUnlocked = true; // 권총은 기본적으로 잠금 해제 상태
        }
    }
}
