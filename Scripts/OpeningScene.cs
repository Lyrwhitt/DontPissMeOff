using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

    public void LoadMainScene()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("GameScene");
    }
}
