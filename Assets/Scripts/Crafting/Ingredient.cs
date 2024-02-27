using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;
    
    public void initialize(Sprite spr, int count)
    {
        sprite = spr;

        image.sprite = sprite;
        countText.text = count.ToString();
    }

}
