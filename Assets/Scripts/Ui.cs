using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour {
    public Text CurrentTime;
    public Text Happiness;
    public Text HappinessPerDay;
    public Text Day;
    public Text Money;
    public Text Dialogue;
    public RectTransform DialogueTransform;
    public WebBrowser WebBrowser;

    public string DayFormat;
    public string HappinessPerDayFormat;

    public float DialogueDisplayTime;

	// Use this for initialization
	void Start ()
    {
        HideDialogue();
    }
    
    float dialogueTimeRemaining;
    public void ShowDialogue(string dialogue)
    {
        Dialogue.transform.parent.gameObject.SetActive(true);
        Dialogue.text = dialogue;
        dialogueTimeRemaining = DialogueDisplayTime / Game.TimeScale;
    }

    public void HideDialogue()
    {
        Dialogue.transform.parent.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        DialogueTransform.anchoredPosition = Game.Player.transform.position;

        if(dialogueTimeRemaining > 0)
        {
            dialogueTimeRemaining -= Time.deltaTime;

            if (dialogueTimeRemaining <= 0)
            {
                HideDialogue();
            }
        }

        if(Day == null)
        {
            // Weird ass Unity bug leaves all these null on first frame
            return;
        }

        Day.text = string.Format(DayFormat, Game.Day);
        CurrentTime.text = Game.Time.ToShortTimeString();
        Happiness.text = Game.Player.Happiness.ToString();
        Money.text = Game.Player.Money.ToString();

        bool hpdIsNegative = Game.Player.HappinessPerDay < 0;

        HappinessPerDay.text = string.Format(HappinessPerDayFormat, (hpdIsNegative ? Color.red : Color.green).ToHex(), hpdIsNegative ? "" : "+", Game.Player.HappinessPerDay);
    }
}
