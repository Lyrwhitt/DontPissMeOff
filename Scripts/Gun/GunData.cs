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
        DrawDefaultInspector(); // �Ϲ����� GunData �ν����� ǥ��

        // ������ ���, ���� ��ź���� ��ź ���� ���� �ν����Ϳ� ����
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
    // �ٸ� �� ����
}

public enum FireMode
{
    SINGLE,
    AUTOMATIC
}
