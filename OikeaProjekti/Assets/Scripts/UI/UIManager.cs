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
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Button ButtonSettingsClose;
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
        ButtonMainMenuStartGame.onClick.AddListener(() => {
            Score.Reset();         //REPLACE WITH RELEVANT WHEN IM UP TO HERE WITH DATABASE
            SceneManager.LoadSceneAsync("Level1");

        });
        ButtonMainMenuQuit.onClick.AddListener(()=> Application.Quit());
        ButtonMainMenuSettings.onClick.AddListener(()=> settingsCanvas.gameObject.SetActive(true));
        ButtonSettingsClose.onClick.AddListener(   ()=> settingsCanvas.gameObject.SetActive(false));
        Score.GameCompleted.Subscribe(b => ShowCongrats(b));
    }



    //kinda temporary logic, here until i have reworked things from the Hole class into here
    void ShowCongrats(bool value)
    {
        Debug.Log("Congrats dealt with");
        UICompletion.gameObject.SetActive(value);
        congratsText.text = $"YOUR SCORE WAS:\r\n{Score.Total}\r\n\r\n Prev Best:\r\n{Score.HighScore}";
    }

    void OnSceneChanged(Scene curr, Scene next){
        if(next.buildIndex == 0)
        {
            UIMainMenu.gameObject.SetActive(true);
        }
    }
    void OnEnable() => SceneManager.activeSceneChanged += OnSceneChanged;
    void OnDisable() => SceneManager.activeSceneChanged -= OnSceneChanged;

}
