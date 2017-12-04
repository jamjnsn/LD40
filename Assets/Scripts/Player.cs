using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SerializedMonoBehaviour {
    public enum DialogueLines { BedSoon, WakingUp, GettingHome, BuyingThing }
    public enum PlayerDirections { Forward = 0, Left = -1, Right = 1 }

    public int HappinessPerDay
    {
        get
        {
            int hpd = 0;
            foreach(Thing thing in Things)
            {
                if(thing.Item != null && thing.Item.IsCool)
                {
                    hpd += thing.HappinessPerDay;
                }
            }
            return hpd + AmbientHappinessPerDay;
        }
    }

    public float X
    {
        get
        {
            return transform.position.x;
        }

        set
        {
            transform.position = new Vector2(value, transform.position.y);
        }
    }

    public int AmbientHappinessPerDay;
    public int AmbientHappinessIncreasePerDay;
    
    [OdinSerialize]
    public Dictionary<DialogueLines, string[]> Dialogue;

    [Range(0,1000)]
    public int Happiness;
    
    [SerializeField]
    public ThingList Things = new ThingList();
    public int Money;

    [Header("Settings")]
    public float MoveSpeed;
    public float MoveSnap;
    public int Paycheck;

    [SerializeField]
    [ReadOnlyAttribute]
    bool moving;
    public bool Moving
    {
        get
        {
            return moving;
        }

        set
        {
            moving = value;
            animator.SetBool("Walking", value);
        }
    }

    [SerializeField]
    [ReadOnlyAttribute]
    PlayerDirections facing;
    public PlayerDirections Facing
    {
        get
        {
            return facing;
        }

        set
        {
            facing = value;
            animator.SetInteger("Direction", (int)value);
        }
    }
    
    Vector2 destination;

    [SerializeField]
    Animator animator;

    public void Buy(Thing thing)
    {
        Money -= thing.Item.Price;
    }

    public void Acquire(Thing thing)
    {
        if (!Things.Contains(thing))
        {
            thing.Acquired = true;
            Things.Add(thing);

            if(thing.Item != null && thing.Item.IsCool)
            {
                AmbientHappinessIncreasePerDay--;
            }

            if (thing.Item.Replaces != null)
            {
                Unacquire(Game.Things.Find(thing.Item.Replaces));
            }
        }
    }

    public void Refund(Thing thing)
    {
        Money += thing.Item.Price;
    }

    public void Unacquire(Thing thing)
    {
        if(Things.Contains(thing))
        {
            thing.Acquired = false;
            Things.Remove(thing);

            if (thing.Item != null && thing.Item.IsCool)
            {
                AmbientHappinessIncreasePerDay++;
            }
        }
    }

    public void Repair(Thing thing)
    {
        thing.Repair();
    }

    public void Show()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void NextDay()
    {
        Happiness += HappinessPerDay;
        AmbientHappinessPerDay += AmbientHappinessIncreasePerDay;

        foreach(Thing thing in Things)
        {
            thing.Novelty -= thing.Item.NoveltyLossPerDay;
            thing.Health = Mathf.Clamp(thing.Health - thing.Item.DamagePerDay, 0, thing.Item.MaxHealth);
        }
    }

    public void GetPaid()
    {
        Money += Paycheck;
    }

    public void Say(DialogueLines concept)
    {
        if(Dialogue.ContainsKey(concept))
        {
            string[] lines = Dialogue[concept];
            int lineNumber = Random.Range(0, lines.Length);
            string line = lines[lineNumber];
            Game.Ui.ShowDialogue(line);
        }
        else
        {
            Debug.LogWarningFormat("Dialogue for {0} not implemented.", concept);
        }
    }

	// Use this for initialization
	void Awake ()
    {
        animator.SetTrigger("Initialize");
    }
	
	// Update is called once per frame
	void Update ()
    {
        animator.speed = Time.timeScale;
        if (Moving)
        {
            Vector2 distance = destination - transform.position.ToVector2();
            Vector2 direction = distance.normalized;
            
            Vector2 position = transform.position.ToVector2() + direction * MoveSpeed * Game.TimeScale * Time.deltaTime;
            
            if(distance.x < 0 && position.x < destination.x ||
                distance.x > 0 && position.x > destination.x)
            {
                position.x = destination.x;
            }

            if (distance.y < 0 && position.y < destination.y ||
                distance.y > 0 && position.y > destination.y)
            {
                position.y = destination.y;
            }

            if (position == destination)
            {
                Moving = false;
            }

            transform.position = position;
        }
	}
    
    public void MoveTo(Vector2 destination)
    {
        this.destination = destination;

        if(destination.x < transform.position.x)
        {
            Facing = PlayerDirections.Left;
        }
        else if(destination.x > transform.position.y)
        {
            Facing = PlayerDirections.Right;
        }

        Moving = true;
    }

    public void MoveTo(float destinationX)
    {
        MoveTo(new Vector2(destinationX, transform.position.y));
    }

    public void MoveTo(Thing thing)
    {
        if(Things.Contains(thing))
        {
            MoveTo(thing.transform.position.x);
        }
        else
        {
            Debug.LogErrorFormat("Player doesn't have a {0}!", thing.name);
        }
    }

    public void MoveTo(string thingName)
    {
        Thing thing = Game.Things.FindByName(thingName);
        if(thing == null)
        {
            Debug.LogErrorFormat("{0} is not a thing that exists!", thingName);
        }
        else
        {
            MoveTo(thing);
        }
    }

    public void WakeUp()
    {
        animator.SetTrigger("Toggle Bed");
    }

    public void Sleep()
    {
        animator.SetInteger("Direction", 0);
        animator.SetTrigger("Toggle Bed");
    }

    public void Sit()
    {
        animator.SetInteger("Direction", 0);
        animator.SetTrigger("Toggle Chair");
    }

    public void Stand()
    {
        animator.SetTrigger("Toggle Chair");
    }
}
