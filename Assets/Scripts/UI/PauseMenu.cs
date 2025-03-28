using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuWindow;
    public GameObject SettingMenuWindow;

    public void MenuButton()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void SettingButton()
    {
        SettingMenuWindow.SetActive(true);
    }
    public void BackButton()
    {
        Time.timeScale = 1f;
        PauseMenuWindow.SetActive(false);
    }
}
