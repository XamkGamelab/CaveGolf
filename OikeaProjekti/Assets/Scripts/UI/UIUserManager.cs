using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class UIUserManager : MonoBehaviour
{
    //all relevant UI items
    [Header("base sign in menu")]
    public RectTransform SignInSignUpPanel;
    public InputField LoginUsername;
    public InputField LoginPassword;
    public Button SignInButton;
    public Button ButtonOpenUserSignupWindow;

    [Header("User creation menu")]
    public RectTransform UserCreationPanel;
    public RectTransform UserCreationSucceessPanel;
    public RectTransform UserCreationFailPanel;
    public InputField NewUsername;
    public Text UserCreationEmailErrorText;
    public InputField NewPassword;
    public Text UserCreationPasswordErrorText;
    public Button ButtonNewUser;

    [Header("User panel")]
    public RectTransform UserPanel;
    public Text   TextUserPanelUsername;
    public Button ButtonUserPanelLogout;
    [Header("Debug")]
    public Button ButtonCreateDebugUser;
    public Button ButtonLoginDebugUser;
    // public bool DebugLoginEnabled;
    public string DebugUsername = "Debug@testi.com";
    public string DebugPassword = "TestiTestiTesti";
    public void ShowLeaderboard()
    {
        Database.Instance.GetUserDetails(()=> null);
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
        Database.Instance.User.Subscribe(_ => OnLogInStatusChanged(Database.Instance.SignedIn));
        Database.Instance.User.Subscribe(u => OnUserNameChanged(u?.Email));

        //Check user input as the user types using  reactive observables
        NewUsername.OnValueChangedAsObservable().Subscribe(username => VerifyEmail(username));
        NewPassword.OnValueChangedAsObservable().Subscribe(password => VerifyPassword(password));

        if (!Database.Instance.SignedIn) {SignInSignUpPanel.gameObject.SetActive(true);}
    }
    //Helper function to organize adding listeneres to all buttons
    void InitButtons()
    {
        // SignInButton.onClick.AddListener(() => Database.Instance.DebugSetSignedIn());
        ButtonOpenUserSignupWindow.onClick.AddListener(() => UserCreationPanel.gameObject.SetActive(true));
        ButtonUserPanelLogout.onClick.AddListener(() => Database.Instance.SignOut());
        ButtonNewUser.onClick.AddListener(() => {
            //only try logging in if username and password are valid-ish (firebase does its own validation)
            if (isValidEmail(NewUsername.text) && isValidPassword(NewPassword.text))
            {
                Debug.Log("Trying to log in");
                Database.Instance.SignUp(NewUsername.text, NewPassword.text, AccountCreationErrorCallback);
            }
            else {
                Debug.LogWarning("Invalid Username or password?");
            }
        });
        ButtonCreateDebugUser.onClick.AddListener(()=>  Database.Instance.SignUp(DebugUsername,DebugPassword, AccountCreationErrorCallback));
        ButtonLoginDebugUser.onClick.AddListener(()=>  {
            Database.Instance.SignIn(DebugUsername,DebugPassword,LoginErrorCallback);
            });
        SignInButton.onClick.AddListener(() => Database.Instance.SignIn(LoginUsername.text,LoginPassword.text,LoginErrorCallback));
    }

    void VerifyEmail(string username)
    {
        Debug.Log("Verifying email: " +username);
        if (isValidEmail(username))
        {
            UserCreationEmailErrorText.gameObject.SetActive(false);
        }
        else
        {
            UserCreationEmailErrorText.gameObject.SetActive(true);
            UserCreationEmailErrorText.text = "Warning: entered address may be invalid!";
        }
    }
    void VerifyPassword(string password)
    {
        Debug.Log("Verifying password: " + password);
        if (isValidPassword(password))
        {            
            UserCreationPasswordErrorText.gameObject.SetActive(false);
        }
        else
        {
            UserCreationPasswordErrorText.gameObject.SetActive(true);
            UserCreationPasswordErrorText.text = "PASSWORD IS TOO SHORT!";
        }
    }
    //really basic regular expression, just checks that the result is of  the form "thing@domain.TLD"
    bool isValidEmail(string email) => new Regex("^\\S+@\\S+\\.\\S+$").IsMatch(email) | email.Length < 2;
    bool isValidPassword(string password) => password.Length > 6;

    void OnLogInStatusChanged(bool isSignedIn)
    {
        SignInSignUpPanel.gameObject.SetActive(!isSignedIn);
        UserCreationPanel.gameObject.SetActive(false);
        NewUsername.text =   "";
        NewPassword.text =   "";
        LoginUsername.text = "";
        LoginPassword.text = "";
        UserPanel.gameObject.SetActive(isSignedIn);
    }
    void OnUserNameChanged(string username)
    {
        if (username is null) return;
        TextUserPanelUsername.text = username.Substring(0,username.IndexOf("@"));
    }

    public void LoginErrorCallback(System.Exception e)
    {
        UserCreationFailPanel.gameObject.SetActive(true);

    }
    public void AccountCreationErrorCallback(System.Exception e)
    {
        UserCreationFailPanel.gameObject.SetActive(true);
    }
}
