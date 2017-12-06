using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public static Player Player { get; private set; }
    public static CameraController Camera { get; private set; }
    public static Ui Ui { get; private set; }
    public static DateTime Time { get; private set; }
    public static int Day { get; private set; }
    public static ThingList Things { get; private set; }

    public static float TimeScale {
        get
        {
            return Instance.gameTimeScale;
        }
    }
    
    delegate bool WaitCondition();

    [SerializeField]
    Transform roomTransform;

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

    [SerializeField]
    public float gameTimeScale;

    public int LossHappiness;
    public int WinHappiness;

    public string StartTime;
    public string FastForwardButton;
    public int DaysPerPaycheck;

    SceneLoader sceneLoader;
    
    [OnValueChanged("UpdateAway")]
    [SerializeField]
    bool away;

    public bool Away
    {
        get
        {
            return away;
        }

        set
        {
            away = value;
            UpdateAway();
        }
    }

    [OnValueChanged("UpdateTimeScale")]
    [SerializeField]
    bool fastForward;
    public bool FastForward
    {
        get
        {
            return fastForward;
        }

        set
        {
            fastForward = value;
            UpdateTimeScale();
        }
    }

    [SerializeField]
    float bedOffset;

    /// <summary>
    /// Unity time scale when player is at work or asleep.
    /// </summary>
    [OnValueChanged("UpdateTimeScale")]
    public float FastForwardMultiplier;
    
    // Use this for initialization
    void Awake () {
        sceneLoader = GetComponent<SceneLoader>();
        Instance = this;
        Time = DateTime.Parse(StartTime);
        Day = 0;
        Player = player;
        Camera = camera;
        Things = things;
        Ui = ui;
        
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Thing"))
        {
            Thing thing = gameObject.GetComponent<Thing>();

            if(thing.Item != null && thing.Acquired)
            {
                Player.Acquire(thing);
            }

            Things.Add(thing);
        }

        Away = false;
        Camera.FollowTarget = roomTransform;
        Camera.SnapToTarget();

        Player.Money += Player.Paycheck;

        Player.X = things.FindByName("Bed").transform.position.x + bedOffset;

        NewDay();
	}

    private void Start()
    {
        //Ui.WebBrowser.Open();
    }

    // Update is called once per frame
    void Update() {
        if(!Away)
        {
            FastForward = Input.GetButton(FastForwardButton);
        }

        DateTime previousTime = Time;
        Time = Time.AddSeconds((double)(UnityEngine.Time.deltaTime * gameTimeScale));

        if (previousTime.Day != Time.Day)
        {
            NewDay();
        }
    }

    bool PayDay
    {
        get
        {
            return Day % DaysPerPaycheck == 0;
        }
    }

    /// <summary>
    /// Increase day and setup daily events.
    /// </summary>
    void NewDay()
    {
        Day++;
        Player.NextDay();

        if (player.Happiness <= LossHappiness)
        {
            sceneLoader.Lose();
        }
        else if (player.Happiness >= WinHappiness)
        {
            sceneLoader.Win();
        }
        else
        {
            // Paycheck
            if (PayDay)
            {
                Player.GetPaid();
            }

            SetUpDay();
        }
    }

    void SetUpDay()
    {
        // Wake up
        WaitForTime("7:55am", () =>
        {
            Away = false;
        });

        WaitForTime("8:00am", () =>
        {
            Player.WakeUp();
            Player.Unacquire(Things.FindByName("Bed With Player"));
        });


        WaitForTime("8:04am", () => {
            Player.Say(Player.DialogueLines.WakingUp);
        });

        // Walk to bathroom
        WaitForTime("8:10am", () =>
        {
            Player.MoveTo("Sink");
            if(PayDay)
            {
                Player.Say(Player.DialogueLines.PayDay);
            }
        });

        WaitForTime("8:25am", () =>
        {
            Player.MoveTo("Door");
            WaitForMove(() =>
            {
                Player.Hide();
            });
        });

        WaitForTime("8:30am", () =>
        {
            Away = true;
        });
        
        WaitForTime("5:25pm", () =>
        {
            Away = false;
            Player.Facing = Player.PlayerDirections.Left;
        });

        WaitForTime("5:30pm", () =>
        {
            Player.Say(Player.DialogueLines.GettingHome);
            Player.MoveTo("Computer");
            WaitForMove(() =>
            {
                Player.Sit();
            });
        });

        WaitForTime("5:35pm", () =>
        {
            Ui.WebBrowser.Open();
        });

        WaitForTime("9:30pm", () =>
        {
            Player.Say(Player.DialogueLines.BedSoon);
        });

        WaitForTime("9:50pm", () =>
        {
            Ui.WebBrowser.Close();
        });

        WaitForTime("9:55pm", () =>
        {
            Player.Stand();
        });

        WaitForTime("10:00pm", () =>
        {
            Player.MoveTo(new Vector2(things.FindByName("Bed").transform.position.x + bedOffset, Player.transform.position.y));
            WaitForMove(() =>
            {
                // go to bed
                Player.Sleep();
            });
        });

        WaitForTime("10:04pm", () =>
        {
            Player.Acquire(Things.FindByName("Bed With Player"));
        });

        WaitForTime("10:05pm", () =>
        {
            Camera.FollowTarget = roomTransform;
        });

        WaitForTime("10:10pm", () =>
        {
            Away = true;
        });
    }

    public void WaitForMove(Action callback)
    {
        StartCoroutine(DoWaitForMove(callback));
    }

    IEnumerator DoWaitForMove(Action callback)
    {
        while(Player.Moving)
        {
            yield return null;
        }

        callback.TryInvoke();
    }

    public void WaitForTime(string time, Action callback)
    {
        StartCoroutine(DoWaitForTime(time, Day, callback));
    }

    IEnumerator DoWaitForTime(string time, int currentDay, Action callback)
    {
        TimeSpan waitTime = DateTime.Parse(time).TimeOfDay;
        if (Time.TimeOfDay > waitTime)
        {
            yield break;
        }
        else
        {
            while (Time.TimeOfDay < waitTime && Day <= currentDay)
            {
                yield return null;
            }

            callback.TryInvoke();
        }
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
        UnityEngine.Time.timeScale = FastForward ? FastForwardMultiplier : 1;
    }

    void UpdateAway()
    {
        UpdateTimeScale();
        FastForward = Away;

        if (Away)
        {
            Player.Hide();
            Camera.FollowTarget = roomTransform;
        }
        else
        {
            Player.Show();
            Camera.FollowTarget = Player.transform;
        }
    }

    void UpdateTime()
    {
        Time = DateTime.Parse(currentTime);
    }
}
