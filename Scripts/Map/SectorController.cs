using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SectorController : MonoBehaviour
{
    const string DATA_PATH = "Datas/EnemySpawnDatas";
    const string DATA_PATH_PREFAB = "Prefabs/";
    const string DATA_PATH_SO = "Datas/EnemySODatas";

    private EnemySpawnDatas _spawnDatas;
    private EnemySODatas _enemyDatas;

    public int enemyCount = 0;
    public int sectorIndex = 0;

    private GameObject[] _enemyPrefabMelee;
    private GameObject[] _enemyPrefabSniper;
    private GameObject[] _enemyPrefabStandingSniper;
    private GameObject[] _enemyPrefabRifle;
    private GameObject[] _enemyPrefabElite;

    public List<GameObject> sectorWalls = new List<GameObject>();
    public List<GameObject> sectorItems = new List<GameObject>();
    private List<GameObject> _spawnedEnemies = new List<GameObject>(); 

    public List<SectorData> sectorDataList = new List<SectorData>();

    WaitUntil waitUntil;

    private void Awake()
    {
        _spawnDatas = Resources.Load<EnemySpawnDatas>(DATA_PATH);
        _enemyDatas = Resources.Load<EnemySODatas>(DATA_PATH_SO);

        _enemyPrefabMelee = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "Melee/"));
        _enemyPrefabSniper = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "Sniper/"));
        _enemyPrefabStandingSniper = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "StandingSniper/"));
        _enemyPrefabRifle = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "Rifle/"));
        _enemyPrefabElite = Resources.LoadAll<GameObject>(string.Concat(DATA_PATH_PREFAB, "Elite/"));
        InitializeSectorData(0);
    }

    private void Start()
    {
        StartNextSector();
    }

    public void OnRetry()
    {
        CharacterController controller = GameManager.Instance.player.transform.GetComponent<CharacterController>();
        SaveData savedData = GameManager.Instance.LoadLastData();

        sectorIndex = savedData.sectorCount;

        if (sectorIndex < sectorDataList.Count) // 이 부분을 추가합니다.
        {
            CinemachinePOV pov = GameManager.Instance.player.firstCamera.GetCinemachineComponent<CinemachinePOV>();
            SectorData sectorData = sectorDataList[sectorIndex];

            pov.m_VerticalAxis.Value = sectorData.playerSpawnRot.x;
            pov.m_HorizontalAxis.Value = sectorData.playerSpawnRot.y;

            controller.enabled = false;
            GameManager.Instance.player.transform.position = sectorData.playerSpawnPos;
            controller.enabled = true;

            for (int i = 0; i < _spawnedEnemies.Count; i++)
            {
                Destroy(_spawnedEnemies[i]);
            }

            InitializeSectorData(sectorIndex);
            StartNextSector();

            for (int i = 0; i < sectorIndex; i++)
            {
                SectorData data = sectorDataList[i];

                if (data.hasEvent)
                {
                    for (int j = 0; j < data.eventScenario.Count; j++)
                    {
                        data.eventScenario[j].gameObject.SetActive(false);
                    }
                }

                for (int j = 0; j < data.sectorWalls.Count; j++)
                {
                    data.sectorWalls[j].SetActive(false);
                }
                for (int j = 0; j < data.sectorItems.Count; j++)
                {
                    data.sectorItems[j].SetActive(false);
                }
            }
        }
    }

    // 다음 섹터 시작
    private void StartNextSector()
    {
        List<EnemySpawnData> datas = _spawnDatas.spawnDatas.FindAll((x) => x.sectorIdx == sectorIndex);

        for(int i = 0; i < datas.Count; i++)
        {
            EnemySpawnData data = datas[i];

            SpawnEnemie(data);
        }
    }

    
    public void SpawnEnemie(EnemySpawnData spawnData)
    {
        GameObject enemy = null;

        switch (spawnData.statsChangeType)
        {
            case StatsChangeType.Melee:
                enemy = _enemyPrefabMelee[Random.Range(0, _enemyPrefabMelee.Length)];
                enemy.GetComponent<Enemy>().Data = _enemyDatas.EnemyMeleeSO;
                break;
            case StatsChangeType.Rifle:
                enemy = _enemyPrefabRifle[Random.Range(0, _enemyPrefabRifle.Length)];
                enemy.GetComponent<Enemy>().Data = _enemyDatas.EnemyBaseSO;
                break;
            case StatsChangeType.Sniper:
                enemy = _enemyPrefabSniper[Random.Range(0, _enemyPrefabSniper.Length)];
                enemy.GetComponent<Enemy>().Data = _enemyDatas.EnemySniperSO;
                break;
            case StatsChangeType.Sniper2:
                enemy = _enemyPrefabStandingSniper[Random.Range(0, _enemyPrefabStandingSniper.Length)];
                enemy.GetComponent<Enemy>().Data = _enemyDatas.EnemySniperSO;
                break;
            case StatsChangeType.Elite:
                enemy = _enemyPrefabElite[Random.Range(0, _enemyPrefabElite.Length)];
                enemy.GetComponent<Enemy>().Data = _enemyDatas.EnemyEliteSO;
                break;
        }

        if (enemy == null)
            return;

        enemy = Instantiate(enemy, spawnData.position, Quaternion.identity);

        Health enemyHealth = enemy.GetComponent<Health>();
        enemyHealth.OnDie += OnEnemyDied;
        enemyCount++;

        _spawnedEnemies.Add(enemy);
    }
    
    private void OnSectorClear()
    {
        if (sectorDataList[sectorIndex].hasEvent)
        {
            //sectorDataList[sectorCount].eventScenario.StartEventMode();
            waitUntil = new WaitUntil(CheckLinkedEventClear);
            StartCoroutine(WaitForEvent());
        }
        else
        {
            OnClearSector();    
            sectorIndex++;

            SetNextSector();
        }
    }

    private bool CheckLinkedEventClear()
    {
        for (int i = 0; i < sectorDataList[sectorIndex].eventScenario.Count; i++)
        {
            if (!sectorDataList[sectorIndex].eventScenario[i].isClear)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator WaitForEvent()
    {
        OnClearSector();

        yield return waitUntil;

        sectorIndex++;

        SetNextSector();
    }

    private void OnClearSector()
    {
        foreach (GameObject wall in sectorWalls)
        {
            wall.SetActive(false);
        }
    }

    private void InitializeSectorData(int sectorIndex)
    {
        sectorWalls.Clear();
        sectorItems.Clear();
        _spawnedEnemies.Clear();

        enemyCount = 0;

        if (sectorIndex >= 0 && sectorIndex < sectorDataList.Count)
        {
            // 참조가 아닌 값만 복사하도록 변경
            sectorWalls = sectorDataList[sectorIndex].sectorWalls.ToList();
            sectorItems = sectorDataList[sectorIndex].sectorItems.ToList();
        }
        else
        {
            // 유효하지 않은 인덱스에 대한 처리를 추가
        }
    }

    private void SetNextSector()
    {
        GameManager.Instance.SaveLastData();
        for (int i = 0; i < _spawnedEnemies.Count; i++)
        {
            Destroy(_spawnedEnemies[i], 5f);
        }

        InitializeSectorData(sectorIndex);
        StartNextSector();
    }
    public void OnEnemyDied()
    {
        enemyCount--;

        if (enemyCount == 0)
        {
            OnSectorClear();
        }
    }
}
