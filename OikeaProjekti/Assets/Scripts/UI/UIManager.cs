using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
public class UIManager : SingletonMono<UIManager>
{
    [SerializeField]UIUserManager UserManagerPrefab;
    [SerializeField] bool debugHideLogin;
    [Header("After completion congratulations")]
    [SerializeField]Canvas UICompletion;
    [SerializeField] TextMeshProUGUI congratsText;
    [Header("Main Menu")]
    [SerializeField] Canvas UIMainMenu;
    [SerializeField] Button ButtonMainMenuStartGame,ButtonMainMenuLeaderboard,ButtonMainMenuSettings,ButtonMainMenuCredits, ButtonMainMenuQuit;
    [Header("Settings")]
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Button ButtonSettingsClose;
    [Header("Credits")]
    [SerializeField] Canvas CreditsCanvas;
    [SerializeField] Button ButtonCloseCredits;
    [Header("Pause")]
    [SerializeField] PauseMenu pauseMenu;


    [HideInInspector]public UIUserManager userManager;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        userManager = Instantiate(UserManagerPrefab,gameObject.transform);
        if(debugHideLogin) userManager.gameObject.SetActive(false);
        UICompletion.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            Score.SaveHighScore();//REPLACE WITH RELEVANT WHEN IM UP TO HERE WITH DATABASE
            Score.Reset();        //REPLACE WITH RELEVANT WHEN IM UP TO HERE WITH DATABASE
            ShowCongrats(false);
        } );
        ButtonMainMenuStartGame.onClick.AddListener(async () => {
            Score.Reset();         //REPLACE WITH RELEVANT WHEN IM UP TO HERE WITH DATABASE
            await SceneManager.LoadSceneAsync("Level1");
            UIMainMenu.gameObject.SetActive(false);
        });
        ButtonMainMenuQuit.onClick.AddListener(()=> Application.Quit());
        ButtonMainMenuSettings.onClick.AddListener(()=> settingsCanvas.gameObject.SetActive(true));
        ButtonSettingsClose.onClick.AddListener(   ()=> settingsCanvas.gameObject.SetActive(false));
        Score.GameCompleted.Subscribe(b => ShowCongrats(b));
        ButtonMainMenuCredits.onClick.AddListener(() => CreditsCanvas.gameObject.SetActive(true));
        ButtonCloseCredits.onClick.AddListener(() => CreditsCanvas.gameObject.SetActive(false));
    }



    //kinda temporary logic, here until i have reworked things from the Hole class into here
    void ShowCongrats(bool value)
    {
        Debug.Log("Congrats dealt with");
        UICompletion.gameObject.SetActive(value);
        congratsText.text = $"YOUR SCORE WAS:\r\n{Score.Total}\r\n\r\n Prev Best:\r\n{Score.HighScore}";
    }

    void OnSceneChanged(Scene curr, Scene next){
        if(next.buildIndex != 0) UIMainMenu.gameObject.SetActive(false);
        if (next.buildIndex == 0)
        {
            UIMainMenu.gameObject.SetActive(true);
        }
    }
    void OnEnable() => SceneManager.activeSceneChanged += OnSceneChanged;
    void OnDisable() => SceneManager.activeSceneChanged -= OnSceneChanged;
    [RuntimeInitializeOnLoadMethod]
    static void BootStrap()
    {
        GameObject g = (GameObject)Resources.Load("UIManager");
        Instantiate(g);
    }
}
