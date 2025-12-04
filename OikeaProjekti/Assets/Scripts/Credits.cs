using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject credits;
    [SerializeField] GameObject settings;

    public void CreditsOpen()
    {
        credits.SetActive(true);
    }
    public void CreditsClose()
    {
        credits.SetActive(false);
    }
    public void SettingsOpen()
    {
        settings.SetActive(true);
    }
    public void SettingsClose()
    {
        settings.SetActive(false);
    }
}