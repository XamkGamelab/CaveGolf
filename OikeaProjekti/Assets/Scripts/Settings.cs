using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject settings;
    [SerializeField] GameObject muteToggle;
    [SerializeField] PauseMenu pauseMenu;

    public void SettingsOpen()
    {
        pauseMenu.pauseMenu.SetActive(false);
        settings.SetActive(true);
       
    }
    public void SettingsClose()
    {
        settings.SetActive(false);
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
