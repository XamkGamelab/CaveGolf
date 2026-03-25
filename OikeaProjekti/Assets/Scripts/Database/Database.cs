using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class Database : Singleton<Database>
{
    public bool SignedIn;

    Firebase.FirebaseApp app;

    public Database()
    {
        Debug.Log("INIT DATABASE");
        CheckDependencyStatus();
    }
    async public void SignUp(string email, string password, Utils.ErrorCallback errorCallback)
    {
        Debug.Log($"Attempting to create an account with email ${email} and password ${password}");
        if(app is null){
            System.Exception e = new System.NullReferenceException("Signup failed because firebase app was null");
            Debug.LogException(e);
            errorCallback(e);
            return;
        }

        Debug.Log("Login task starting");
        Task createUser = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);
        try
        {
            await createUser;

        }
        catch (Exception e)
        {
            Debug.Log("ERROR: " + e.Message);
            errorCallback(e);
            return;
        }
        Debug.Log("Task Done");
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