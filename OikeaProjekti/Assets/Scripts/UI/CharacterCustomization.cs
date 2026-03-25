using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.UI;
public class CharacterCustomization : MonoBehaviour
{
    public Renderer ballRenderer;
    UnityEngine.UI.Image MainMenuImage;
    bool MainMenu = false;
    public ColorButton[] colorButtons;

    private Dictionary<string, string> colors =
        new Dictionary<string, string>() {
            {"Green", "#00DD20FF"},
            {"Blue",  "#00F2FFFF"},
            {"Pink",  "#FF00E5FF"}
    };

    private Color currentColor;

    private void Awake()
    {        
        if (ballRenderer == null)
        {
            GameObject ball = GameObject.FindGameObjectWithTag("Player");
            ballRenderer = ball.GetComponent<Renderer>();
            if(ballRenderer == null)
            {
                MainMenuImage = ball.GetComponent<UnityEngine.UI.Image>();
            }
        }

        if (PlayerPrefs.HasKey("SelectedColor"))
        {
            SelectColor(PlayerPrefs.GetString("SelectedColor"));
        }
        else
        {
            SelectColor("Green");
        }
    }
    public void SelectColor(string colorName)
    {
        if (colors.ContainsKey(colorName))
        {
            string hex = colors[colorName];
            ColorUtility.TryParseHtmlString(hex, out currentColor);

            if(ballRenderer != null)
            {
                ballRenderer.material.color = currentColor;
            }
            else
            {
                MainMenuImage.color = currentColor;
            }
            PlayerPrefs.SetString("SelectedColor", colorName);

            Debug.Log("Väri vaihdettu: " + colorName);
        }
        else
        {
            Debug.LogWarning("Väriä ei löytynyt: " + colorName);
        }
        foreach (var btn in colorButtons)
        {
            btn.UpdateState(colorName);
        }
    }
}