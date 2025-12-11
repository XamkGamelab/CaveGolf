using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public Image targetImage;
    public string colorName;
    public Sprite normalSprite;
    public Sprite selectedSprite;

    private Image img;

    /*private void Awake()
    {
        img = GetComponent<Image>();
    }*/

    public void UpdateState(string selectedColor)
    {
        if (targetImage == null)
        {
            Debug.LogError("ColorButton: targetImage puuttuu " + gameObject.name);
            return;
        }

        targetImage.sprite = (selectedColor == colorName) ? selectedSprite : normalSprite;
    }
}
