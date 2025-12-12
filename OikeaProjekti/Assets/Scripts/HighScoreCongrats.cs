using TMPro;
using UnityEngine;

public class HighScoreCongrats : MonoBehaviour
{

    // things used by instances of script, should not be accessed elsewhere

    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    GameObject canvas;



    void Start()
    {
        if (!Score.GameCompleted)
        {
            canvas.SetActive(false);

        }
        if (Score.HighScore != 0 || Score.Total != 0)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
        text.text = $"YOUR SCORE WAS:\r\n{Score.Total}\r\n\r\n Prev Best:\r\n{Score.HighScore}";
        Score.SaveHighScore();
        Score.Reset();
    }

}
