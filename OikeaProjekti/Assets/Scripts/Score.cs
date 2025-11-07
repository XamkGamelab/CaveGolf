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
    {
        text.text = " " + Total.ToString() + (Local != 0 ? ("<br>+" + Local.ToString()) : "");
    }
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UI_UPADTE();
        if (Instance != null) Debug.LogWarning("Replacing existing Score instance with new one, there may be multiple in scene?");
        Instance = this;
    }
}
