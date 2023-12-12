using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPause : UIPopup
{
    public Button resumeBtn;
    public Button settingBtn;
    public Button quitBtn;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(OnResumeBtnClicked);
        settingBtn.onClick.AddListener(OnSettingBtnClicked);
        quitBtn.onClick.AddListener(OnQuitBtnClicked);
    }

    private void OnResumeBtnClicked()
    {
        GameManager.Instance.ResumeGame();
        UIManager.Instance.ClosePopup();
    }

    private void OnSettingBtnClicked()
    {
        if (!UIManager.Instance.IsOpenedPopup("UISetting"))
        {
            UIManager.Instance.ShowPopup<UISetting>();
            this.gameObject.SetActive(false);
        }
    }

    private void OnQuitBtnClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("StartScene");
    }
}
