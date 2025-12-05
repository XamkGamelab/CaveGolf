using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject credits;

    public void CreditsOpen()
    {
        credits.SetActive(true);
    }
    public void CreditsClose()
    {
        credits.SetActive(false);
    }
}