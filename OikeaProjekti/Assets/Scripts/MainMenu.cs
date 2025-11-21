using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Score.Reset();
        SceneManager.LoadSceneAsync("Level1");
    }
}