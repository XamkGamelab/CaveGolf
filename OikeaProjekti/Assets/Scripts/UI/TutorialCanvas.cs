using UnityEngine;

public class FadeText : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            tutorialCanvas.SetActive(false);
        }
    }
}
