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

namespace Data
{
    /// <summary>
    /// Class encapsulating all direct interaction with remote Google Firebase Database
    /// </summary>
    public class Database : Singleton<Database>
    {
        Firebase.FirebaseApp app;
        /// <summary>
        /// Is the User currently signed in with a valid account?
        /// </summary>
        /// <value> Current login status</value>
        public bool IsSignedIn => User.HasValue && User.Value != null && User.Value.IsValid();
        /// <summary>
        /// Current user and it's details.
        /// a UniRx *ReactiveProperty*, which allows it to be subscribed to
        /// so that logins/logouts can be dynamically responded to elsewhere
        /// </summary>
        /// <value>Currently Signed in user</value>
        public ReactiveProperty<FirebaseUser> User { get; private set; } = new();

        /// <summary>
        /// Asynchronous task that returns the current **FULL** leaderboard from server, sorted by score.
        /// </summary>
        /// <returns>Full sorted leaderboard</returns>
        /// <remarks>Anything that wishes to use this ought to filter it down/ further process it if desired</remarks>
        /// <remarks>This unfortunately has issues if the leaderboards is empty :( havent fixed yet</remarks>
        /// <seealso cref="Data.Leaderboard"/>
        /// <seealso cref="Data.LeaderboardEntry"/>
        public async Task<Leaderboard> GetLeaderboardAsync()
        {
            var ds = ReadLeaderboardAsync();
            await ds;
            string updateResult = "{\"leaderboards\":" + ds.Result.GetRawJsonValue() + "}";
            if (updateResult is null || updateResult.Length == 0) return null;
            Debug.Log("JSON : " + updateResult);
            Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(updateResult);
            leaderboard.leaderboards = leaderboard.leaderboards.OrderBy(l => l.Score).ToList();
            return leaderboard;
        }
        /// <summary>
        /// Fetches the current user's details, and returns them via a callback action
        /// </summary>
        /// <param name="callback">Callback that the details will be returned to</param>
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

        /// <summary>
        /// Saves the score given into the current users details / leaderboard
        /// </summary>
        /// <param name="score">int of the current score to be saved</param>
        public void RecordScore(int score)
        {
            AddScoreToLeaders(Utils.DbUtils.EmailToUsername(User.Value.Email), score);
        }
        /******************************************************************
        ***
        ***     Login/Logout user management section
        ***
        ******************************************************************/
        /// <summary>
        /// Tries to authenticate user into Firebase with given details
        /// </summary>
        /// <param name="email">email address of user</param>
        /// <param name="password">password of user</param>
        /// <param name="errorCallback">errorCallback to an error handler in calling ui</param>
        /// <seealso cref="Utils.ErrorCallback"/>
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
            Debug.Log(" Signed in");
        }
        /// <summary>
        /// Signs Current user out
        /// </summary>
        async public void SignOut()
        {
            Debug.Log("Signed Out");
            FirebaseAuth.DefaultInstance.SignOut();
            User.Value = null;
        }
        /// <summary>
        /// Attempt to create a new user with given information
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password"><user password/param>
        /// <param name="errorCallback">errorCallback to an error handler in calling ui</param>
        /// <seealso cref="Utils.ErrorCallback"/>
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


        /// <summary>
        /// Updates leaderboard and user highscore if the given highscore is best
        /// </summary>
        /// <param name="username">username to update with</param>
        /// <param name="score">score</param>
        /// <remarks>Has a few funny pieces of functionality: Updates score to leaderboard only if it is better than user's previous score.</remarks>
        /// <remarks>There can only be Leaderboard.MaxEntries leaderboard entries stored at once. If the new score is the worst, it wont be saved</remarks>
        private async void AddScoreToLeaders(string username, int score)
        {
            DatabaseReference leaderBoardRef = FirebaseDatabase.DefaultInstance.RootReference.Child("leaderboards");
            //Check if current user exists on server
            //  if not, try to create a valid one
            //  if is, check if new score is the highest attained
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

                    if (u.bestscore <= score) return; //user already had better score, early out
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

                if (leaders == null)
                {
                    leaders = new List<object>();
                }
                //if leaderboard is at max capacitty, try and find a way to create the new entry
                else if (mutableData.ChildrenCount >= Leaderboard.MaxEntries)
                {
                    //LINQ hell that gets the leaderboard entry with the lowest score
                    object entryWithWorstScore = leaders.OrderBy(entry => (long)((Dictionary<string, object>)entry)["score"]).FirstOrDefault();
                    long worstScore = (long)((Dictionary<string, object>)entryWithWorstScore)["score"];

                    //new score is worse than existing scores, so we abort
                    if (worstScore > score)
                        return TransactionResult.Abort();
                    else
                        leaders.Remove(entryWithWorstScore);
                }

                //we now know that the score can safely be added to leaderboard
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
        }
        /// <summary>
        /// Force sets a users details in the database
        /// </summary>
        /// <param name="LoggedinUser"></param>
        /// <param name="user"></param>
        async void SetUserRecord(FirebaseUser LoggedinUser, UserDetails user)
        {
            string json = JsonUtility.ToJson(user);
            string userId = LoggedinUser.UserId;
            await FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
        }


        public Database()
        {
            Debug.Log("INIT DATABASE");
            CheckDependencyStatus();
            User.Value = null;
        }
        //private get of any arbitrary location in the database
        private Task<DataSnapshot> ReadDatabaseAsync(string path)
        {
            return FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync();
        }
        //privately reads the database
        private Task<DataSnapshot> ReadUserAsync()
        {
            return ReadDatabaseAsync("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/");
        }
        //private get of Firebase leaderboard structure
        private Task<DataSnapshot> ReadLeaderboardAsync()
        {
            return FirebaseDatabase.DefaultInstance.RootReference.Child("leaderboards").GetValueAsync();
        }
        /// <summary>
        /// verifies that everything with the firebase plugin is enabled correctly.
        /// If it fails, no further
        /// </summary>
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
    }
}