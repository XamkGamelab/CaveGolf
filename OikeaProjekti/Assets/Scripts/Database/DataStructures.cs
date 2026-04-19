using System.Collections.Generic;
namespace Data
{

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
}
