using UnityEngine;
using UnityEngine.UI;
public class UILeaderboardEntry : MonoBehaviour
{
    public Text UsernameText;
    public Text ScoreText;
    public UILeaderboardEntry Init(Data.LeaderboardEntry entry)
    {
        UsernameText.text = entry.Username;
        ScoreText.text = entry.Score.ToString();
        return this;
    }
}