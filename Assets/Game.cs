using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour {
    static int minutesPerDay = 1440;

    float elapsedTime;

    public int Day { get; private set; }

    /// <summary>
    /// Current time in minutes.
    /// </summary>
    /// 
    //DateTime time;
    int time;
    public int GameTime
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
            if(time >= minutesPerDay)
            {
                Day++;
                GameTime = time - minutesPerDay;
            }
            else if(time < 0)
            {
                Day--;
                GameTime = minutesPerDay + time;
            }
        }
    }

    public int CurrentMinute
    {
        get
        {
            return GameTime % 60;
        }

        set
        {
            GameTime = CurrentHour * 60 + value;
        }
    }

    public int CurrentHour
    {
        get
        {
            return Mathf.FloorToInt((float)GameTime / 60f);
        }

        set
        {
            GameTime = value * 60 + CurrentMinute;
        }
    }

    delegate bool WaitCondition();

    public static Player Player;
    public static CameraController Camera;
    public static List<Thing> Things;

    [SerializeField]
    Player player;
    [SerializeField]
    CameraController camera;

    public int StartTime;

    /// <summary>
    /// Realtime minutes to game minutes.
    /// </summary>
    public float MinuteScale;

    /// <summary>
    /// Unity time scale when player is at work or asleep.
    /// </summary>
    public float AwayTimeScale;

    public static int HourToMinutes(int hour)
    {
        return hour * 60;
    }

    public static int MinutesToHour(int minutes)
    {
        return Mathf.FloorToInt((float)minutes / 60);
    }

    // Use this for initialization
    void Awake () {
        GameTime = StartTime;
        Player = player;
        Camera = camera;
	}

    // Update is called once per frame
    void Update() {
        //time.AddSeconds(elapsedTime);
        elapsedTime += Time.deltaTime;
        GameTime = StartTime + (int)(elapsedTime / 60 * MinuteScale);
        Debug.Log(TimeString);
    }

    public string TimeString
    {
        get
        {
            return string.Format("{0:00}:{1:00}", CurrentHour, CurrentMinute);
        }
    }

    public void WaitForGameTime(int gameTime, Action callback)
    {
        StartCoroutine(DoWaitForGameTime(gameTime, Day, callback));
    }

    IEnumerator DoWaitForGameTime(int gameTime, int currentDay, Action callback)
    {
        while (GameTime < gameTime && Day <= currentDay)
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
}
