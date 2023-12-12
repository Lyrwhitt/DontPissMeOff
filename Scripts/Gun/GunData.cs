#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/Gun Data", order = 1)]
public class GunData : ScriptableObject
{
    public string gunName;
    public int damage;
    public float bulletSpeed;
    public float reloadTime;
    public float fireInterval;
    public float equipTime;
    public float scopeFactor;
    [Range(0, 1f)] public float baseAccuracy;
    [Range(0, 1f)] public float inaccuracy;
    public int magazineSize;
    public GunType gunType;
    public FireMode fireMode;

    [HideInInspector] public int shotgunPellets;
    [HideInInspector] public float shotgunSpread;
    [HideInInspector] public int grenadeCount = 0;
}

#if UNITY_EDITOR
[CustomEditor(typeof(GunData))]
public class GunDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GunData gunData = (GunData)target;
        DrawDefaultInspector(); // 일반적인 GunData 인스펙터 표시

        // 샷건일 경우, 샷건 산탄수와 산탄 범위 변수 인스펙터에 노출
        if (gunData.gunType == GunType.SHOTGUN)
        {
            gunData.shotgunPellets = EditorGUILayout.IntField("Shotgun Pellets", gunData.shotgunPellets);
            gunData.shotgunSpread = EditorGUILayout.FloatField("Shotgun Spread", gunData.shotgunSpread);
        }

        if (gunData.gunType == GunType.GRENADE)
        {
            gunData.grenadeCount = EditorGUILayout.IntField("Grenade Count", gunData.grenadeCount);
        }
    }
}
#endif

public enum GunType
{
    PISTOL,
    RIFLE,
    SHOTGUN,
    SNIPER_RIFLE,
    SUB_MACHINE_GUN,
    GRENADE
    // 다른 총 종류
}

public enum FireMode
{
    SINGLE,
    AUTOMATIC
}
