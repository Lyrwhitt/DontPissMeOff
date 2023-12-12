using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPausePopup();
        }
    }

    [Header("Guide")]
    [field: HideInInspector] public GuideController guideController;


    [field: Header("Pause")]
    public bool isPause = false;
    public event UnityAction onPause;
    public event UnityAction onResume;
    public event UnityAction onRetry;


    public SectorController currentSector;
    public EventScenario currentEvent;

    public Player player;

    public GameObject playerSet;
    public GameObject cutsceneCam;

    public UIPlayerHit playerHit;
    public UIDamage damageUI;

    public SaveData saveData = new SaveData();

    private void Start()
    {
        SoundManager.Instance.Play("FieldSound", SoundManager.Sound.Bgm);

        guideController = this.GetComponent<GuideController>();

        ResumeGame();
        DataSetting();
    }

    private void DataSetting()
    {
        if (PlayerPrefs.GetString("NewGame") == "True")
        {
            return;
        }
        else if (PlayerPrefs.GetString("NewGame") == "False")
            RetryGame();
    }

    private void ShowPausePopup()
    {
        if (!UIManager.Instance.IsOpenedPopup("UIPause"))
        {
            PauseGame();
            UIManager.Instance.ShowPopup<UIPause>();
        }
    }

    public void PauseGameForTutorial()
    {
        isPause = true;
        Time.timeScale = 0f;

        onPause?.Invoke();
    }

    public void PauseGame()
    {
        isPause = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        onPause?.Invoke();
    }

    public void ResumeGame()
    {
        isPause = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        onResume?.Invoke();
    }

    public void RetryGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        onRetry?.Invoke();
        if (currentEvent == null)
        {
            currentSector.OnRetry();
        }
        else
        {
            currentEvent.OnRetry();
        }
    }

    public void SaveLastData()
    {
        saveData.sectorCount = currentSector.sectorIndex;
        saveData.playerHP = player.status.health;
        string saveDataJson = saveData.Serialize();
        PlayerPrefs.SetString("SaveData", saveDataJson);

        PlayerPrefs.Save();
    }

    public SaveData LoadLastData()
    {
        string saveDataJson = PlayerPrefs.GetString("SaveData", "{}");
        saveData = SaveData.Deserialize(saveDataJson);

        return saveData;
    }
}
