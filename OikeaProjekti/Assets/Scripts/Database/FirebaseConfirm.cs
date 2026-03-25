using UnityEngine;

using System.Collections.Generic;
using UnityEngine.UI;
using Utils;

public class FirebassController : MonoBehaviour
{
    public Button ButtonCreateWindow;

    //public List<LeaderboardEntry> LeaderboardEntries;


    public RectTransform SignInSignUpPanel;
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
        ButtonCreateWindow.onClick.AddListener(() => UserCreationPanel.gameObject.SetActive(true));

        if (!Database.Instance.SignedIn)
        {
            SignInSignUpPanel.gameObject.SetActive(true);
        }
    }

    //void InstantiateLeaderboardEntries(List<LeaderboardData> leaderboardDatas){}

    public void AccountCreationErrorCallback(System.Exception e)
    {
        UserCreationFailPanel.gameObject.SetActive(true);
    }



}
