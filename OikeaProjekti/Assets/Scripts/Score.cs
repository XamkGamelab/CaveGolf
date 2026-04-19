using TMPro;
using UniRx;
using UnityEngine;
namespace Data
{

    public class Score : MonoBehaviour
    {

        public static int Local { get; private set; }
        public static int Total { get; private set; }
        public static ReactiveProperty<bool> GameCompleted = new(false);

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
            GameCompleted.Value = false;
        }
        public static void OnFinishGame()
        {
            GameCompleted.Value = true;
            Data.Database.Instance.RecordScore(Total);
        }
        // things used by instances of script, should not be accessed elsewhere

        [HideInInspector]
        static Score Instance;
        [HideInInspector]
        TextMeshProUGUI text;
        void UI_UPADTE()
        {
            string totalString = Total.ToString();
            string localString = "+" + Local.ToString("#0");

            while (totalString.Length != localString.Length)
            {
                if (totalString.Length > localString.Length)
                {
                    localString = " " + localString;
                }
                else
                {
                    totalString = " " + totalString;
                }
            }
            text.text = totalString + "<br>" + localString;
        }
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            UI_UPADTE();
            Instance = this;
        }
    }
}
