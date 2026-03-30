using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : SingletonMono<UIManager>
{
    [SerializeField]UIUserManager UserManagerPrefab;
    [SerializeField] bool debugHideLogin;
    [Header("After completion congratulations")]
    [SerializeField]Canvas UICompletion;
    [SerializeField] TextMeshProUGUI congratsText;

    [HideInInspector]public UIUserManager userManager;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        userManager = Instantiate(UserManagerPrefab,gameObject.transform);
        if(debugHideLogin) userManager.gameObject.SetActive(false);
        UICompletion.GetComponentInChildren<Button>().onClick.AddListener(() => ShowCongrats(false));
    }


    void ShowCongrats(bool value)
    {
        UICompletion.gameObject.SetActive(value);
        congratsText.text = $"YOUR SCORE WAS:\r\n{Score.Total}\r\n\r\n Prev Best:\r\n{Score.HighScore}";
        Score.SaveHighScore();
        Score.Reset();
    }
}
