using UnityEngine;
using TMPro;
public class ScoreUI : MonoBehaviour
{
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = " "+ Score.Total.ToString() + (Score.Local !=0 ? ("<br>+" +Score.Local.ToString()) :"") ;


        Score.Local.ToString();
        Score.CurrentInstance = this;


    }

    public void UI_UPADTE()
    {
        text.text = " " + Score.Total.ToString() + (Score.Local != 0 ? ("<br>+" + Score.Local.ToString()) : "");
    }
}
