using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class Game : MonoBehaviour
{
    public static Player Player { get; private set; }
    public static CameraController Camera { get; private set; }
    public static Ui Ui { get; private set; }
    public static DateTime Time { get; private set; }
    public static int Day { get; private set; }
    public static ThingList Things { get; private set; }
    
    delegate bool WaitCondition();

    [SerializeField]
    Player player;
    [SerializeField]
    CameraController camera;
    [SerializeField]
    Ui ui;

    [SerializeField]
    [ReadOnlyAttribute]
    ThingList things = new ThingList();

    [SerializeField]
    [OnValueChanged("UpdateTime")]
    string currentTime;

    public string StartTime;
    public Light Sun;
    public float GameTimeScale;
    public int DaysPerPaycheck;

    [OnValueChanged("UpdateTimeScale")]
    public bool Away;

    /// <summary>
    /// Unity time scale when player is at work or asleep.
    /// </summary>
    [OnValueChanged("UpdateTimeScale")]
    public float AwayTimeMultiplier;
    
    // Use this for initialization
    void Awake () {
        Time = DateTime.Parse(StartTime);
        Day = 0;
        Player = player;
        Camera = camera;
        Things = things;
        
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Thing"))
        {
            Things.Add(gameObject.GetComponent<Thing>());
        }

        NewDay();
	}

    // Update is called once per frame
    void Update() {
        DateTime previousTime = Time;
        Time = Time.AddSeconds((double)(UnityEngine.Time.deltaTime * GameTimeScale));

        if (previousTime.Day != Time.Day)
        {
            NewDay();
        }
    }

    /// <summary>
    /// Increase day and setup daily events.
    /// </summary>
    void NewDay()
    {
        Day++;
        Player.Happiness += player.HappinessPerDay;

        if(Day % DaysPerPaycheck == 0)
        {
            Player.GetPaid();
        }

        // Wake up
        WaitForTime("8:00am", () =>
        {
        });

        WaitForTime("12:00pm", () =>
        {
            Debug.Log("It's hiiiiigh noon.");
        });
    }

    public void WaitForTime(string time, Action callback)
    {
        StartCoroutine(DoWaitForTime(time, Day, callback));
    }

    IEnumerator DoWaitForTime(string time, int currentDay, Action callback)
    {
        while (Time.TimeOfDay < DateTime.Parse(time).TimeOfDay && Day <= currentDay)
        {
            yield return null;
        }

        callback.TryInvoke();
    }

    public void Wait(float seconds, Action callback)
    {
        StartCoroutine(DoWait(seconds, callback));
    }

    IEnumerator DoWait(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback.TryInvoke();
    }

    void WaitFor(WaitCondition condition, Action callback)
    {
        StartCoroutine(DoWaitFor(condition, callback));
    }   

    IEnumerator DoWaitFor(WaitCondition waitCondition, Action callback)
    {
        while(waitCondition() == false)
        {
            yield return null;
        }

        callback.TryInvoke();
    }

    void UpdateTimeScale()
    {
        UnityEngine.Time.timeScale = Away ? AwayTimeMultiplier : 1;
    }

    void UpdateTime()
    {
        Time = DateTime.Parse(currentTime);
    }
}
