using UnityEngine;
using UniRx;
using System.Collections.Generic;
using UnityEngine.UI;
using Utils;

public class FirebassController : MonoBehaviour
{
    public Button ButtonCreateWindow;

    //public List<LeaderboardEntry> LeaderboardEntries;


    public RectTransform SignInSignUpPanel;
    public Button SignInButton;



    public RectTransform UserCreationPanel;
    public RectTransform UserCreationSucceessPanel;
    public RectTransform UserCreationFailPanel;
    public InputField NewUsername;
    public InputField NewPassword;
    public Button ButtonNewUser;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ButtonNewUser.onClick.AddListener(()=>{
            Database.Instance.SignUp(NewUsername.text, NewPassword.text, AccountCreationErrorCallback);
        });
        Database.Instance.SignedIn.Subscribe(b => ShowSignInUI(b));

        ButtonCreateWindow.onClick.AddListener(() => UserCreationPanel.gameObject.SetActive(true));

        if (!Database.Instance.SignedIn.Value)
        {
            SignInSignUpPanel.gameObject.SetActive(true);
        }

        SignInButton.onClick.AddListener(() => Database.Instance.DebugSetSignedIn());
    }

    void ShowSignInUI(bool isSignedIn)
    {
        Debug.Log("Signed in? = " + isSignedIn);
        SignInSignUpPanel.gameObject.SetActive(!isSignedIn);

    }


    //void InstantiateLeaderboardEntries(List<LeaderboardData> leaderboardDatas){}

    public void AccountCreationErrorCallback(System.Exception e)
    {
        UserCreationFailPanel.gameObject.SetActive(true);
    }



}
