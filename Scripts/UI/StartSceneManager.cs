using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class StartSceneManager : MonoBehaviour
{
    private bool _isTouched = false;
    public UILoading loadingBar;
    public GameObject touchScreenTextObj;
    public GameObject titleTextObj;
    private float _blinkInterval = 0.5f;
    private float _fadeDuration = 1.0f;

    public Button newGameBtn;
    public Button loadGameBtn;
    public Button optionBtn;
    public Button quitGameBtn;
    public Button resetBtn;

    public RectTransform buttonSetRect;

    private AsyncOperation operation;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SoundManager.Instance.Play("StartBGM", SoundManager.Sound.Bgm);

        loadingBar.gameObject.SetActive(false);

        newGameBtn.onClick.AddListener(OnNewGameBtnClicked);
        loadGameBtn.onClick.AddListener(OnLoadGameBtnClicked);
        optionBtn.onClick.AddListener(OnOptionBtnClicked);
        quitGameBtn.onClick.AddListener(OnQuitGameBtnClicked);
        resetBtn.onClick.AddListener(OnResetBtnClicked);
        if (PlayerPrefs.HasKey("SaveData"))
        {
            loadGameBtn.gameObject.SetActive(true);
            resetBtn.gameObject.SetActive(true);
        }
        else
        {
            loadGameBtn.gameObject.SetActive(false);
            resetBtn.gameObject.SetActive(false);
        }
        StartCoroutine(FadeInTitle());
    }

    private void OnResetBtnClicked()
    {
        PlayerPrefs.DeleteAll();
        loadGameBtn.gameObject.SetActive(false);
    }

    private IEnumerator FadeInTitle()
    {
        CanvasGroup canvasGroup = titleTextObj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = titleTextObj.AddComponent<CanvasGroup>();
        }

        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnOptionBtnClicked()
    {
        SoundManager.Instance.Play("Button1", SoundManager.Sound.Effect);

        if (!UIManager.Instance.IsOpenedPopup("UISetting"))
        {
            UIManager.Instance.ShowPopup<UISetting>();
        }
    }

    private void OnLoadGameBtnClicked()
    {
        SoundManager.Instance.Play("Button1", SoundManager.Sound.Effect);

        PlayerPrefs.SetString("NewGame", "False");
        loadingBar.gameObject.SetActive(true);
        LoadGameScene();
        loadingBar.StartLoading();

        StartCoroutine(WaitLoading());
    }

    private void OnNewGameBtnClicked()
    {
        SoundManager.Instance.Play("Button1", SoundManager.Sound.Effect);

        PlayerPrefs.SetString("NewGame", "True");
        loadingBar.gameObject.SetActive(true);
        LoadOpeningScene();
        loadingBar.StartLoading();

        StartCoroutine(WaitLoading());
    }

    private void OnQuitGameBtnClicked()
    {
        SoundManager.Instance.Play("Button1", SoundManager.Sound.Effect);

        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isTouched)
        {
            SoundManager.Instance.Play("Button1", SoundManager.Sound.Effect);

            touchScreenTextObj.SetActive(false);

            _isTouched = true;

            ShowButtons(true);
        }
        if (!_isTouched)
        {
            float lerpValue = Mathf.PingPong(Time.time / _blinkInterval, 1f);
            touchScreenTextObj.SetActive(lerpValue > 0.5f);
        }
    }

    private void LoadOpeningScene()
    {
        ShowButtons(false);
        operation = SceneManager.LoadSceneAsync("OpeningScene");

        operation.allowSceneActivation = false;
    }
    private void LoadGameScene()
    {
        ShowButtons(false);
        operation = SceneManager.LoadSceneAsync("GameScene");

        operation.allowSceneActivation = false;
    }

    private IEnumerator WaitLoading()
    {
        WaitUntil waitUntil = new WaitUntil(() => operation.progress >= 0.9f && loadingBar.isFinish);

        yield return waitUntil;

        SoundManager.Instance.Clear();

        operation.allowSceneActivation = true;
    }

    private void ShowButtons(bool show)
    {
        if(show)
            buttonSetRect.DOAnchorPosX(0f, 0.5f).SetEase(Ease.OutQuad);
        else
            buttonSetRect.DOAnchorPosX(-512f, 0f).SetEase(Ease.OutQuad);
    }
}
