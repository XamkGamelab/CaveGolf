using UnityEngine;

public class Score
{
    public static int Local { get; private set; }
    public static int Total { get; private set; }
    public static ScoreUI CurrentInstance;

    public static void Add(int i)
    {
        Local += i;
        if (CurrentInstance != null) 
            CurrentInstance.UI_UPADTE();
    }

    public static void UpdateTotal()
    {
        Total += Local;
        Local = 0;
        if (CurrentInstance != null)
            CurrentInstance.UI_UPADTE();
    }

    

    public static void Save()
    {
        Debug.LogError("ERROR: SCORE SAVING IS NOT IMPLEMENTED YET");
    }
    public static void Load()
    {
        Debug.LogError("ERROR: SCORE LOADING IS NOT IMPLEMENTED YET");
    }
}
