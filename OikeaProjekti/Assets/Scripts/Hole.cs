using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter2D()
    {
        Score.UpdateTotal();
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(0);
            Debug.LogError("INVALID SCENE");
        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }



}
