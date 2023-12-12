using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    [Header("Sector Data")]
    public int sectorCount;

    [Header("Player Status")]
    public int playerHP;

    [Header("Unlocked Weapons")]
    public List<GunType> unlockedWeapons = new List<GunType>();
    public int grenadeCount;

    public List<GunType> UnlockedWeapons
    {
        get { return unlockedWeapons; }
    }

    // 직렬화 함수
    public string Serialize()
    {
        SaveDataWrapper wrapper = new SaveDataWrapper(this);
        return JsonUtility.ToJson(wrapper);
    }

    // 역직렬화 함수
    public static SaveData Deserialize(string json)
    {
        SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);
        return wrapper.ToSaveData();
    }

    [Serializable]
    private class SaveDataWrapper
    {
        public List<string> unlockedWeapons;
        public int sectorCount;
        public int playerHP;
        public int grenadeCount;

        public SaveDataWrapper(SaveData saveData)
        {
            unlockedWeapons = saveData.UnlockedWeapons.ConvertAll(gunType => gunType.ToString());
            sectorCount = saveData.sectorCount;
            playerHP = saveData.playerHP;
            grenadeCount = saveData.grenadeCount;
        }

        public SaveData ToSaveData()
        {
            SaveData saveData = new SaveData();
            saveData.UnlockedWeapons.Clear();
            saveData.sectorCount = sectorCount;
            saveData.playerHP = playerHP;
            saveData.grenadeCount = grenadeCount;
            foreach (string gunTypeString in unlockedWeapons)
            {
                if (Enum.TryParse(gunTypeString, out GunType gunType))
                {
                    saveData.UnlockedWeapons.Add(gunType);
                }
            }

            return saveData;
        }
    }
}