using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using Firebase.Database;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


public class UserDetails
{
    public string Username;
    public int bestscore;
    public UserDetails(string _Username, int _score)
    {
        Username = _Username;
        bestscore = _score;
    }
}
public class Leaderboard
{
    public const int MaxEntries = 10;
    public List<LeaderboardEntry> leaderboards = new();
}
[System.Serializable]
public struct LeaderboardEntry
{
    public string Username;
    public int Score;
    public LeaderboardEntry(string username, int score)
    {
        Username = username;
        Score = score;
    }
}

public class Database : Singleton<Database>
{
    public bool SignedIn => User.HasValue && User.Value != null && User.Value.IsValid();
    public ReactiveProperty<FirebaseUser> User { get; private set; } = new();

    Firebase.FirebaseApp app;



    //nonfunctional for now
    public async void GetUserDetails(Action<UserDetails> callback)
    {
        Task<DataSnapshot> t = ReadUserAsync();
        try
        {
            await t;
            if (t.Result == null) return;
            UserDetails u = JsonConvert.DeserializeObject<UserDetails>(t.Result.GetRawJsonValue());
            Debug.Log(t.Result.GetRawJsonValue());
            callback(u);
        }
        catch (Exception ex)
        {
            Debug.LogError("FAILED TO GET USER DETAILS.");
            Debug.LogException(ex);
        }
    }
    public async void GetLeaderboardAsync()
    {
        var ds = ReadLeaderboardAsync();
        await ds;
        Debug.Log(ds.Result.GetRawJsonValue());
    }

    private async void AddScoreToLeaders(string username, int score)
    {
        DatabaseReference leaderBoardRef = FirebaseDatabase.DefaultInstance.RootReference.Child("leaderboards");
        Task<DataSnapshot> t = ReadUserAsync();
        try
        {
            await t;
            if (t.Result == null || t.Result.GetRawJsonValue() == null)
            {
                SetUserRecord(FirebaseAuth.DefaultInstance.CurrentUser, new UserDetails(username, score));
            }
            else
            {
                UserDetails u = JsonConvert.DeserializeObject<UserDetails>(t.Result.GetRawJsonValue());
                Debug.Log($"USER SCORE IN FILE IS: {u.Username} with score {u.bestscore}");
                if (u.bestscore < score) return; //ENABLE THIS BY THE END, DISABLED FOR TESTING
                SetUserRecord(FirebaseAuth.DefaultInstance.CurrentUser, new UserDetails(username, score));
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return;
        }


        Task<DataSnapshot> updateLeaderboards = leaderBoardRef.RunTransaction(mutableData =>
        {
            //All database entries are basically objects with:
            //"Username": string
            //"score": int
            List<object> leaders = mutableData.Value as List<object>;
            //if leaderboard is null we create a new one
            if (leaders == null)
            {
                leaders = new List<object>();
            }
            //if leaderboard is filled, try and find a way to create the new entry
            else if (mutableData.ChildrenCount >= Leaderboard.MaxEntries)
            {
                //LINQ hell that gets the leaderboard entry with the lowest score
                object entryWithWorstScore = leaders.OrderBy(entry => (long)((Dictionary<string, object>)entry)["score"]).FirstOrDefault();
                long minScore = (long)((Dictionary<string, object>)entryWithWorstScore)["score"];

                if (minScore > score)//new score is lower than existing scores, so we abort

                {
                    return TransactionResult.Abort();
                }
                else
                { //We have some lower score thing that can be safely removed
                    leaders.Remove(entryWithWorstScore);
                }
            }
            /////////////////////////////////////////////
            //We can now add our score to the leaderboard
            /////////////////////////////////////////////
            Dictionary<string, object> newScore = new();
            newScore["score"] = score;
            newScore["username"] = username;
            leaders.Add(newScore);
            mutableData.Value = leaders;
            return TransactionResult.Success(mutableData);
        });

        try
        {
            await updateLeaderboards;
        }
        catch (AggregateException ae)
        {
            foreach (Exception ex in ae.InnerExceptions)
            {
                Debug.Log(ex.ToString());
            }
        }
        // string updateResult = "{\"leaderboards\":" +updateLeaderboards.Result.GetRawJsonValue() + "}";
        // Debug.Log("JSON : " + updateResult);
        // Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(updateResult);
        // leaderboard.leaderboards = leaderboard.leaderboards.OrderByDescending(l => l.Score).ToList();
        // Debug.Log("---------------------------------------\n LEADERBOARD OBJECT::" + leaderboard);
        // foreach(LeaderboardEntry l in leaderboard.leaderboards)
        // {
        //     Debug.Log(l.Username + l.Score);
        // }
    }
    async void SetUserRecord(FirebaseUser LoggedinUser, UserDetails user)
    {
        string json = JsonUtility.ToJson(user);
        string userId = LoggedinUser.UserId;
        await FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }




    //******************************************************
    //HANDLES THINGS RELATING TO SIGN IN, SIGN UP AND STARTUP
    //********************************************************
    public Database()
    {
        Debug.Log("INIT DATABASE");
        CheckDependencyStatus();
        User.Value = null;
    }
    private Task<DataSnapshot> ReadDatabaseAsync(string path)
    {
        return FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync();
    }
    private Task<DataSnapshot> ReadUserAsync()
    {
        return ReadDatabaseAsync("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/");
    }
    private Task<DataSnapshot> ReadLeaderboardAsync()
    {
         return FirebaseDatabase.DefaultInstance.RootReference.Child("leaderboards").GetValueAsync();
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
        if (Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser is not null) User.Value = FirebaseAuth.DefaultInstance.CurrentUser;
        Debug.Log("Task Done");
    }
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
        User.Value = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        Debug.Log("Signed in");
        AddScoreToLeaders(email, 3);
    }
    async public void SignOut()
    {
        Debug.Log("Signed Out");
        FirebaseAuth.DefaultInstance.SignOut();
        User.Value = null;
    }
    void CheckDependencyStatus()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {

                Debug.Log("Firebace dependecy status is available");
                app = Firebase.FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    /*******************************************************************
    **OBSOLETE STUFF RELATING TO USER RECORDS KEEPING. 
    **
    async public void SetUserRecord(FirebaseUser LoggedinUser, UserDetails user)
    {
        string json = JsonUtility.ToJson(user);
        string userId = LoggedinUser.UserId;
        await FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
    async public Task<DataSnapshot> ReadDatabase(string path)
    {
        Task<DataSnapshot> task = FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync();
        await task;
        return task.Result;
    }
    async public void GetCurrentUserRecord()
    {
        var ReadDb = ReadDatabase("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/");
        try
        {
            DataSnapshot snapshot = ReadDb.Result;
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
    */
}