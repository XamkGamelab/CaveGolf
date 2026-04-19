using TMPro;
using UniRx;
using UnityEngine;
namespace Data
{

    public class Score : MonoBehaviour
    {

        public static int CurrentLevelScore { get; private set; }
        public static int CurrentRunScore { get; private set; }
        public static ReactiveProperty<bool> GameCompleted = new(false);

        public static void Add(int i)
        {
            CurrentLevelScore += i;
            if (Instance != null)
                Instance.UpdateUI();
        }

        public static void UpdateTotal()
        {
            CurrentRunScore += CurrentLevelScore;
            CurrentLevelScore = 0;
            if (Instance != null)
                Instance.UpdateUI();
        }
        public static void Reset()
        {
            CurrentRunScore = 0;
            CurrentLevelScore = 0;
            Instance?.UpdateUI();
            GameCompleted.Value = false;
        }
        public static void OnFinishGame()
        {
            GameCompleted.Value = true;
            Data.Database.Instance.RecordScore(CurrentRunScore);
        }
        // things used by instances of script, should not be accessed elsewhere

        [HideInInspector]
        static Score Instance;
        [HideInInspector]
        TextMeshProUGUI text;
        void UpdateUI()
        {
            string totalString = CurrentRunScore.ToString();
            string localString = "+" + CurrentLevelScore.ToString("#0");

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
            UpdateUI();
            Instance = this;
        }
    }
}
