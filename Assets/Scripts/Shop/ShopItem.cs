using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI TitleText;

    public void AssignSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            Image.color = Color.clear;
        }
        else
        {
            Image.color = Color.white;
        }

        Image.sprite = sprite;
    }

    public void AssignPrice(string price)
    {
        PriceText.text =  price + " $";
    }
    public void AssignTitle(string name)
    {
        TitleText.text = name.ToString();
    }

}
