using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
