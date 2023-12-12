using UnityEngine;

public class BossSpawnEnemy : MonoBehaviour
{
    const string DATA_PATH = "Datas/Boss/EnemySpawnDatas";
    const string DATA_PATH_PREFAB = "Prefabs/BossEnemy/";

    private EnemySpawnDatas enemySpawnDatas;

    private GameObject[] enemyPrefabMelee;
    private GameObject[] enemyPrefabSniper;
    private GameObject[] enemyPrefabStandingSniper;
    private GameObject[] enemyPrefabRifle;
    private GameObject[] enemyPrefabElite;

    private void Awake()
    {
        enemySpawnDatas = Resources.Load<EnemySpawnDatas>(DATA_PATH);

        enemyPrefabMelee = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "Melee/"));
        enemyPrefabSniper = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "Sniper/"));
        enemyPrefabStandingSniper = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "StandingSniper/"));
        enemyPrefabRifle = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "Rifle/"));
        enemyPrefabElite = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "Elite/"));
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnDatas.spawnDatas.Count; i++)
        {
            SpawnEnemy(enemySpawnDatas.spawnDatas[i]);
        }
    }

    private void SpawnEnemy(EnemySpawnData spawnData)
    {
        GameObject enemy = null;

        switch (spawnData.statsChangeType)
        {
            case StatsChangeType.Melee:
                enemy = enemyPrefabMelee[Random.Range(0, enemyPrefabMelee.Length)];
                break;
            case StatsChangeType.Rifle:
                enemy = enemyPrefabRifle[Random.Range(0, enemyPrefabRifle.Length)];
                break;
            case StatsChangeType.Sniper:
                enemy = enemyPrefabSniper[Random.Range(0, enemyPrefabSniper.Length)];
                break;
            case StatsChangeType.Sniper2:
                enemy = enemyPrefabStandingSniper[Random.Range(0, enemyPrefabStandingSniper.Length)];
                break;
            case StatsChangeType.Elite:
                enemy = enemyPrefabElite[Random.Range(0, enemyPrefabElite.Length)]; ;
                break;
        }

        if (enemy == null)
            return;

        enemy = Instantiate(enemy, spawnData.position, Quaternion.identity, transform);
    }
}
