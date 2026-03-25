using UnityEngine;
using Firebase.Extensions;
using Firebase.Auth;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class FirebassController : MonoBehaviour
{
    public Button ButtonCreateWindow;

    public List<LeaderboardEntry> LeaderboardEntries;
    public RectTransform UserCreationPanel;
    public RectTransform UserCreationSucceessPanel;
    public RectTransform UserCreationFailPanel;
    public InputField NewUsername;
    public InputField NewPassword;
    public Button ButtonNewUser;



    Firebase.FirebaseApp app;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckDependencyStatus();
        ButtonNewUser.onClick.AddListener(SignUp);
        ButtonCreateWindow.onClick.AddListener(() => UserCreationPanel.gameObject.SetActive(true));
    }
    void InstantiateLeaderboardEntries(List<ScoreData> scoredatas){

    }




    async public void SignUp()
    {
        string email = NewUsername.text;
        string password = NewPassword.text;


        Debug.Log("task started");
        Task createUser = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);
        try
        {
        await createUser;

        }
        catch(Exception e)
        {
            Debug.Log("FUCK IT ERRORS: " + e.Message );
            UserCreationFailPanel.gameObject.SetActive(true);
            return;
        }

        Debug.Log("Task Done");
        //await createUser.ContinueWith(t => { });
    }
    void CheckDependencyStatus()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Debug.Log("Firebace dependecy status is available");
                app = Firebase.FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
}
