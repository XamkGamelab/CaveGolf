using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Data;
public class UIUserManager : MonoBehaviour
{
    //all relevant UI items
    [Header("base sign in menu")]
    public RectTransform SignInSignUpPanel;
    public InputField LoginUsername;
    public Button SignInButton;
    public Button ButtonOpenUserSignupWindow;


    [Header("User panel")]
    public RectTransform UserPanel;
    public Text   TextUserPanelUsername;
    public Button ButtonUserPanelLogout;
    // public bool DebugLoginEnabled;
    public string DebugUsername = "Debug@testi.com";
    [Header("Leaderboard")]
    public int MaxLeaderboardEntries = 10;
    public UILeaderboardEntry leaderboardEntryPrefab;
    public RectTransform LeaderboardPanel;
    public VerticalLayoutGroup LeaderboardContainer;
    public Button ButtonLeaderboardClose;
    [SerializeField]
    List<UILeaderboardEntry> leaderboardEntries;

    public async void ShowLeaderboard()
    {
        Leaderboard leaderboard = await Database.Instance.GetLeaderboardAsync();
        if(leaderboard is null) return;
        if(leaderboardEntries is not null)
        {
            foreach(var e in leaderboardEntries)
            {
                Destroy(e.gameObject);
            }
            leaderboardEntries = null;
        }
        leaderboardEntries = new();
        foreach(LeaderboardEntry l in leaderboard.leaderboards.Take(MaxLeaderboardEntries))
        {
            UILeaderboardEntry e = Instantiate<UILeaderboardEntry>(leaderboardEntryPrefab).Init(l);
            e.transform.SetParent(LeaderboardContainer.transform);
            leaderboardEntries.Add(e);
        }
        LeaderboardPanel.gameObject.SetActive(true);
    }
    public void CLoseLeaderboard()
    {
        foreach(var e in leaderboardEntries)
            {
                Destroy(e.gameObject);
            }
            leaderboardEntries = null;
        LeaderboardPanel.gameObject.SetActive(false);
    }



    void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        InitButtons();
        //Only show the login  menu on awake if player is not already logged in.

        /**********************************************************
         * REACTIVE PROPERTIES
         * The Database object, (which is a singleton that deals with login into a Firebase realtime database) has a ReactiveProperty for user's login status
         *          Which is set when the user successfully logs in or logs out
         *          public ReactiveProperty<bool> SignedIn { get; private set; } = new
         * 
         * I subscribe to it here to display/hide a login prompt based on its state
         *********************************************************/
        Database.Instance.CurrentUser.Subscribe(_ => OnLogInStatusChanged(Database.Instance.IsSignedIn));
        Database.Instance.CurrentUser.Subscribe(u => OnUserNameChanged(u));

        //Check user input as the user types using  reactive observables
        if (!Database.Instance.IsSignedIn) {SignInSignUpPanel.gameObject.SetActive(true);}
    }
    //Helper function to organize adding listeneres to all buttons
    void InitButtons()
    {
        // SignInButton.onClick.AddListener(() => Database.Instance.DebugSetSignedIn());
        ButtonUserPanelLogout.onClick.AddListener(() => Database.Instance.SignOut());
        SignInButton.onClick.AddListener(() => Database.Instance.SignIn(LoginUsername.text));
        ButtonLeaderboardClose.onClick.AddListener(()=> CLoseLeaderboard());
    }

    //really basic regular expression, just checks that the result is of  the form "thing@domain.TLD"
    bool isValidEmail(string email) => new Regex("^\\S+@\\S+\\.\\S+$").IsMatch(email) | email.Length < 2;
    bool isValidPassword(string password) => password.Length > 6;

    void OnLogInStatusChanged(bool isSignedIn)
    {
        SignInSignUpPanel.gameObject.SetActive(!isSignedIn);
        LoginUsername.text = "";
        UserPanel.gameObject.SetActive(isSignedIn);
    }
    void OnUserNameChanged(string username)
    {
        if (username is null) return;
        TextUserPanelUsername.text = username;
    }
}
