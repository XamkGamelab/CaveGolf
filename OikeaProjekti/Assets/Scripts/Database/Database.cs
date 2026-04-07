using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using Google.MiniJSON;
using Firebase.Database;
using System.Linq.Expressions;

public class UserDetails
{
    public string Username;
    public float TotalPlayTime;
    public int score;
    public UserDetails(string _Username, float _TotalPlayTime, int _score)
    {
        Username = _Username;
        TotalPlayTime = _TotalPlayTime;
        score = _score;
    }
}

public class Database : Singleton<Database>
{
    //public bool SignedIn;
    public ReactiveProperty<bool> SignedIn { get; private set; } = new();
    public ReactiveProperty<FirebaseUser> User { get; private set; } = new();

    Firebase.FirebaseApp app;

    public Database()
    {
        Debug.Log("INIT DATABASE");
        CheckDependencyStatus();
        SignedIn.Value = false;
        User.Value = null;
    }
    async public void SetUserRecord(FirebaseUser LoggedinUser, UserDetails user)
    {
            string json = JsonUtility.ToJson(user);
            string userId = LoggedinUser.UserId;
            await FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
            Debug.Log(user);
    }
    async public void GetCurrentUserRecord()
    {
        Task<DataSnapshot> task = 
            FirebaseDatabase.DefaultInstance
            .GetReference("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/")
            .GetValueAsync();
        await task;
        try
        {
            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log("SHAPSHOT: " + snapshot.GetRawJsonValue());
            }
        }
        catch
        {
            Debug.Log("FAILED GETTING USER");
        }
    }
    async public void SignUp(string email, string password, Utils.ErrorCallback errorCallback)
    {
        Debug.Log($"Attempting to create an account with email ${email} and password ${password}");
        if (app is null)
        {
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
        if(Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser is not null) User.Value = FirebaseAuth.DefaultInstance.CurrentUser;
        SignedIn.Value = true;
        Debug.Log("Task Done");

        SetUserRecord(User.Value, new UserDetails(email,Time.time,1));
    }
    //private void AuthStateChanged(object sender, EventArgs e) { 
    //    FirebaseAuth auth = FirebaseAuth.DefaultInstance;
    //    bool signedIn = User.Value != auth.CurrentUser && auth.CurrentUser != null && auth.CurrentUser.IsValid();
    //    if(!signedIn && User != null)
    //    {
    //        Debug.Log("Signed out" + User.Value.UserId);
    //    }
    //    User.Value = auth.CurrentUser;
    //    if (signedIn)
    //    {
    //        Debug.Log("Signed in" + User.Value.UserId);
    //    }
    //}
    async public void SignIn(string email, string password, Utils.ErrorCallback errorCallback)
    {
        Debug.Log($"Attempting to sign in with email ${email} and password ${password}");
        if (app is null)
        {
            System.Exception e = new System.NullReferenceException("Signup failed because firebase app was null");
            Debug.LogException(e);
            errorCallback(e);
            return;
        }
        Task signIn = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password);
        try
        {
            await signIn;
        }
        catch (Exception e)
        {
            Debug.Log("ERROR: " + e.Message);
            errorCallback(e);
            return;
        }
        SignedIn.Value = true;
        User.Value = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        Debug.Log("Signed in");
    }
    async public void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SignedIn.Value = false;
        User = null;
    }
    public void DebugSetSignedIn()
    {
        SignedIn.Value = true;
    }
    void CheckDependencyStatus()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
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