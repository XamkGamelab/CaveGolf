using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject settings;
    [SerializeField] GameObject muteToggle;

    public void SettingsOpen()
    {
        settings.SetActive(true);
    }
    public void SettingsClose()
    {
        settings.SetActive(false);
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
