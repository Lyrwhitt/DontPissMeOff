using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [field: HideInInspector] public Player player;

    public UISelectGun selectGunUI;
    public UICrossHair crossHairUI;
    public UIPlayerStatus playerStatusUI;
    public UIGameOver gameOverUI;
    public UIEventMode eventModeUI;

    private void Start()
    {
        InitUISet();

        player = GameManager.Instance.player;
        player.status.OnDie += PlayerOnDeath;
        GameManager.Instance.onRetry += PlayerOnRetry;
    }

    private void PlayerOnDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        selectGunUI.gameObject.SetActive(false);
        eventModeUI.gameObject.SetActive(false);
        crossHairUI.gameObject.SetActive(false);

        gameOverUI.gameObject.SetActive(true);
        gameOverUI.StartCounting();
    }

    private void PlayerOnRetry()
    {
        selectGunUI.gameObject.SetActive(true);

        if (player.isEventMode)
            eventModeUI.gameObject.SetActive(true);

        crossHairUI.gameObject.SetActive(true);
    }

    private void InitUISet()
    {
        eventModeUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }
}
