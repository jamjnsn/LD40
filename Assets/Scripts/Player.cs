using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int HappinessPerDay
    {
        get
        {
            int hpd = 0;
            foreach(Thing thing in Things)
            {
                hpd += thing.HappinessPerDay;
            }
            return hpd + AmbientHappinessPerDay;
        }
    }

    public int AmbientHappinessPerDay;
    public int AmbientHappinessIncreasePerDay;

    [Range(0,1000)]
    public int Happiness;
    
    public ThingList Things = new ThingList();
    public int Money;

    [Header("Settings")]
    public float MoveSpeed;
    public float MoveSnap;
    public int Paycheck;

    public bool IsMoving { get; private set; }

    Vector2 destination;

    public void Acquire(Thing thing)
    {
        thing.Acquired = true;
        Things.Add(thing);
    }

    public void Unacquire(Thing thing)
    {
        thing.Acquired = false;
        Things.Remove(thing);
    }

    public void Repair(Thing thing)
    {
        thing.Repair();
    }

    public void NextDay()
    {
        Happiness += HappinessPerDay;
        AmbientHappinessPerDay += AmbientHappinessIncreasePerDay;
    }

    public void GetPaid()
    {
        Money += Paycheck;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(IsMoving)
        {
            transform.position = Vector2.Lerp(transform.position, destination, MoveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) <= MoveSnap)
            {
                IsMoving = false;
            }
        }
	}
    
    public void MoveTo(Vector2 destination)
    {
        this.destination = destination;
    }

    public void MoveTo(float destinationX)
    {
        MoveTo(new Vector2(destinationX, transform.position.y));
    }

    public void MoveTo(Thing thing)
    {
        if(Things.Contains(thing))
        {
            MoveTo(thing.transform.position);
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
}
