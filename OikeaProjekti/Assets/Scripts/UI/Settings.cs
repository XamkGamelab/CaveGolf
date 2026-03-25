using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject settings;
    [SerializeField] GameObject muteToggle;
    [SerializeField] PauseMenu pauseMenu;

    public void SettingsOpen()
    {
        if (pauseMenu != null)
            pauseMenu.pauseMenu.SetActive(false);

        settings.SetActive(true);
        Debug.Log("SettingsOpen called");
    }

    public void SettingsClose()
    {
        settings.SetActive(false);

        if (pauseMenu != null)
            pauseMenu.pauseMenu.SetActive(true);
    }
    /*public void ToggleOn()
    {
        muteToggle = false;
    }
    public void ToggleOff()
    {
        muteToggle = true;
    }*/
}
