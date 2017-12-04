using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Product : MonoBehaviour {
    public Text Name;
    public Image Image;
    public Text Price;
    public Text Happiness;
    public Button BuyButton;

    public Thing Thing;

    public void SetThing(Thing thing)
    {
        Thing = thing;
        Name.text = thing.Item.Description;
        Price.text = "$" + thing.Item.Price.ToString();
        Image.sprite = thing.Item.Sprite;
        Happiness.text = thing.Item.HappinessPerDay.ToString();
    }

    public void Buy()
    {
        Game.Player.Acquire(Thing);
        Game.Player.Buy(Thing);
        Destroy(this.gameObject);
    }
}
