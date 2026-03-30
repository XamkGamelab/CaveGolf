using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : SingletonMono<UIManager>
{
    [SerializeField]UIUserManager UserManagerPrefab;
    [SerializeField] bool debugHideLogin;
    [Header("After completion congratulations")]
    [SerializeField]Canvas UICompletion;
    [SerializeField] TextMeshProUGUI congratsText;
    [Header("Main Menu")]
    [SerializeField] Canvas UIMainMenu;

    [HideInInspector]public UIUserManager userManager;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        userManager = Instantiate(UserManagerPrefab,gameObject.transform);
        if(debugHideLogin) userManager.gameObject.SetActive(false);
        UICompletion.GetComponentInChildren<Button>().onClick.AddListener(() => ShowCongrats(false));
    }






    //kinda temporary logic, here until i have reworked things from the Hole class into here
    void ShowCongrats(bool value)
    {
        UICompletion.gameObject.SetActive(value);
        congratsText.text = $"YOUR SCORE WAS:\r\n{Score.Total}\r\n\r\n Prev Best:\r\n{Score.HighScore}";
        Score.SaveHighScore();
        Score.Reset();
    }
    void OnSceneChanged(Scene curr, Scene next)
    {
        // = game completed
        if(curr.buildIndex == SceneManager.sceneCountInBuildSettings - 1 && next.buildIndex == 0)
        {
            ShowCongrats(true);
        }
    }
    void OnEnable() => SceneManager.activeSceneChanged += OnSceneChanged;
    void OnDisable() => SceneManager.activeSceneChanged += OnSceneChanged;

}
