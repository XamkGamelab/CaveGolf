using System;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Data
{
    /// <summary>
    /// Class encapsulating all direct interaction with remote Google Firebase Database
    /// </summary>
    public class Database : Singleton<Database>
    {
        public string LeaderboardPath = "leaderboard.json";
        /// <summary>
        /// Is the User currently signed in with a valid account?
        /// </summary>
        /// <value> Current login status</value>
        public bool IsSignedIn => CurrentUser.HasValue && CurrentUser.Value != null;


        /// <summary>
        /// Current user and it's details.
        /// a UniRx *ReactiveProperty*, which allows it to be subscribed to
        /// so that logins/logouts can be dynamically responded to elsewhere
        /// </summary>
        /// <value>Currently Signed in users name</value>
        public ReactiveProperty<string> CurrentUser { get; private set; } = new();
        /// <summary>
        /// Saves the given score into local database, under the current user.
        /// </summary>
        /// <param name="score">score to be saved</param>
        public async void RecordScore(int score)
        {
            Debug.Log("Recording score");
            Leaderboard l =await GetLeaderboardAsync();
            try
            {
                var userRecord = l.leaderboards.Find( u => u.Username == CurrentUser.Value);
                userRecord.Score = score;
            }
            catch
            {
                l.leaderboards.Add(new LeaderboardEntry(CurrentUser.Value, score));
            }
            WriteLeaderboardAsync(l);

        }
        /// <summary>
        /// Signs in to the database (sets the username)
        /// </summary>
        /// <param name="username"></param>
        public void SignIn(string username)
        {
            CurrentUser.Value = username;
        }
        public void SignOut()
        {
            CurrentUser.Value = null;
        }
        /// <summary>
        /// Gets local leaderboard from storage.
        /// Returns null if none saved
        /// </summary>
        /// <returns></returns>
        public async Task<Leaderboard> GetLeaderboardAsync()
        {
            try
            {
                using StreamReader reader = new(LeaderboardPath);
                string rawJson = await reader.ReadToEndAsync();
                Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(rawJson);
                leaderboard.leaderboards = leaderboard.leaderboards?.OrderBy(l => l.Score).ToList();
                return leaderboard;
            }
            catch
            {
                return new Leaderboard();

            }
        }
        private async void WriteLeaderboardAsync(Leaderboard l)
        {
            try
            {
                string s = JsonConvert.SerializeObject(l);  
                await File.WriteAllTextAsync(LeaderboardPath,s);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        public Database()
        {
            Debug.Log("INIT DATABASE");
            CurrentUser.Value = null;
        }
    }
}