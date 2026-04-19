using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using System;
using Data;
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
    [Header("Misc")]
    [SerializeField] Canvas TutorialCanvas;

    [HideInInspector]public UIUserManager userManager;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        userManager = Instantiate(UserManagerPrefab,gameObject.transform);
        if(debugHideLogin) userManager.gameObject.SetActive(false);
        UICompletion.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
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
        Score.GameCompleted.Where(b=> b).Subscribe(b => ShowCongrats(b));
        ButtonMainMenuCredits.onClick.AddListener(() => CreditsCanvas.gameObject.SetActive(true));
        ButtonMainMenuLeaderboard.onClick.AddListener(() => userManager.ShowLeaderboard());
        ButtonCloseCredits.onClick.AddListener(() => CreditsCanvas.gameObject.SetActive(false));
        OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }


    void DisplayTutorial()
    {
        TutorialCanvas.gameObject.SetActive(true);

        //fade out and disable the tutorial on click, only slightly cursed of a reactive setup
        var click = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).First();
        click.Subscribe(_ =>
        {
            Debug.Log("Hide Tutorial", this);
            Observable.EveryFixedUpdate().TakeUntil(Observable.Timer(TimeSpan.FromSeconds(1)))
                .Scan(0f, (total, _) => total + Time.fixedDeltaTime)
                .Subscribe(t => TutorialCanvas.GetComponent<CanvasGroup>().alpha = 1 - t);
        });
        click.Delay(TimeSpan.FromSeconds(1)).Subscribe(_ => TutorialCanvas.gameObject.SetActive(false));
    }



    //kinda temporary logic, here until i have moved some of the scoring features into a more sensible format

    //to be called on when the game has been completed
    void ShowCongrats(bool value)
    {
        Debug.Log("Congrats dealt with");
        UICompletion.gameObject.SetActive(value);
        congratsText.text = $"YOUR SCORE WAS:\r\n{Score.Total}\n   Check Leaderboard to compare!";
    }

    void OnSceneChanged(Scene curr, Scene next){
        if (next.buildIndex == 0)
        {
            pauseMenu.transform.parent.gameObject.SetActive(false);
            UIMainMenu.gameObject.SetActive(true);
        }
        else
        {
            pauseMenu.transform.parent.gameObject.SetActive(true);
            UIMainMenu.gameObject.SetActive(false);
        }
        if (next.buildIndex == 1)
        {
            DisplayTutorial();
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
