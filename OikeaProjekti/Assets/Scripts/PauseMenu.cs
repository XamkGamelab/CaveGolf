using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    public void Home()
    {

    }

    public void Resume()
    {

    }

    public void Restart()
    {

    }
}
