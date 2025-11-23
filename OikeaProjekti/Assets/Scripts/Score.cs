using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{

    public static int Local { get; private set; }
    public static int Total { get; private set; }



    public static void Add(int i)
    {
        Local += i;
        if (Instance != null)
            Instance.UI_UPADTE();
    }

    public static void UpdateTotal()
    {
        Total += Local;
        Local = 0;
        if (Instance != null)
            Instance.UI_UPADTE();
    }
    public static void Reset()
    {
        Total = 0;
        Local = 0;
        Instance?.UI_UPADTE();
    }

    public static void Save()
    {
        Debug.LogError("ERROR: SCORE SAVING IS NOT IMPLEMENTED YET");
    }
    public static void Load()
    {
        Debug.LogError("ERROR: SCORE LOADING IS NOT IMPLEMENTED YET");
    }



    // things used by instances of script, should not be accessed elsewhere

    [HideInInspector]
    static Score Instance;
    [HideInInspector]
    TextMeshProUGUI text;
    void UI_UPADTE()
    {   string totalString = Total.ToString();
        string localString = "+" + Local.ToString("#0");

        while (totalString.Length != localString.Length)
        {
            if(totalString.Length > localString.Length)
            {
                localString = " " + localString;
            }
            else
            {
            totalString = " " + totalString;
            }
        }

        //text.text = $"TOTAL: {Total.ToString()} \n {(Local != 0 ? ("Level:" + Local.ToString()) : "")}";
        text.text =  totalString + (Local != 0 ? ("<br>" + localString) : "");
    }
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UI_UPADTE();
        //if (Instance != null) Debug.LogWarning("Replacing existing Score instance with new one, there may be multiple in scene?");
        Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
}
