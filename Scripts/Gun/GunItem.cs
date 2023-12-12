using UnityEngine;

public class GunItem : MonoBehaviour
{
    public GunData grenadeThrower;
    public GunType gunType;
    private UISelectGun selectGun;

    private void Start()
    {
        selectGun = FindObjectOfType<UISelectGun>();

        if (gunType == GunType.GRENADE && grenadeThrower.grenadeCount < 0)
        {
            grenadeThrower.grenadeCount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (!PlayerPrefs.HasKey("WeaponGuide"))
                GameManager.Instance.guideController.StartGuide(Resources.Load<GuideData>("Guide/WeaponGuide"));
            if (gunType == GunType.GRENADE)
            {
                grenadeThrower.grenadeCount++;
                GameManager.Instance.saveData.grenadeCount = grenadeThrower.grenadeCount;
            }
            selectGun.UnlockWeapon(gunType);
            gameObject.SetActive(false);
        }
        else
            return;
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(GunItem))]
//public class GunItemEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        GunItem gunItem = (GunItem)target;
//        DrawDefaultInspector(); // 일반적인 GunItem 인스펙터 표시

//        // 수류탄일 경우 수류탄 획득 시 GrenadeThrower를 인스펙터에 표시
//        if (gunItem.gunType == GunType.GRENADE)
//        {
//            gunItem.grenadeThrower = EditorGUILayout.ObjectField("Grenade Thrower", gunItem.grenadeThrower, typeof(GunData), true) as GunData;
//        }
//    }
//}
//#endif
