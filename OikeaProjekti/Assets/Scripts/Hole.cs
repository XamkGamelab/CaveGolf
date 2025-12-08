using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Hole : MonoBehaviour
{
    [SerializeField]
    TextMeshPro flagText;
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

    private void Awake()
    {
        if (flagText == null) Debug.LogError("Flag text invalid");

        flagText.text = SceneManager.GetActiveScene().buildIndex.ToString();
    }
}
