using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter2D()
    {
            if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings)
            {
            Debug.LogError("INVALID SCENE");
            }
            else SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1);
    }
}
