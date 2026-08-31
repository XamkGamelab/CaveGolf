using System.Collections.Generic;
namespace Data
{
    public class Leaderboard
    {
        public const int MaxEntries = 10;
        public List<LeaderboardEntry> leaderboards = new();
    }
    [System.Serializable]
    public class LeaderboardEntry
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
