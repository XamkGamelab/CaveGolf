using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    void Start()
    {
        root = uiDocument.rootVisualElement;

        var playButton = root.Q<VisualElement>("PlayButton");

        playButton.RegisterCallback<ClickEvent>(OnClickEvent);
    }

    public void OnClickEvent(ClickEvent evt) { 
        SceneManager.LoadScene("Level1");
    }
}
