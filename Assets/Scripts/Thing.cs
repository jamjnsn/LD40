using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour {
    [SerializeField]
    [OnValueChanged("UpdateItem")]
    Item item;

    public bool IsItem(Item item)
    {
        return this.item == item;
    }
    
    public string Name
    {
        get
        {
            return item.name;
        }
    }

    public bool IsCool
    {
        get
        {
            return item.IsCool;
        }
    }

    [OnValueChanged("UpdateHealth")]
    public int Health;
    
    [OnValueChanged("UpdateAcquired")]
    [SerializeField]
    bool acquired;

    bool isAcquired;
    public bool Acquired
    {
        get
        {
            return isAcquired;
        }

        set
        {
            isAcquired = value;
            gameObject.SetActive(value);
        }
    }

    public float Decay;

    public int HappinessPerDay
    {
        get
        {
            return Mathf.RoundToInt((float)item.HappinessPerDay * Decay * HealthPercentage);
        }
    }

    public float HealthPercentage
    {
        get
        {
            return Health / item.MaxHealth;
        }
    }

    public bool Broken
    {
        get
        {
            return Health == 0;
        }
    }


    public void Repair()
    {
        Health = item.MaxHealth;
    }

	// Use this for initialization
	void Start () {
        Acquired = acquired;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateHealth()
    {
        Health = Mathf.Clamp(Health, 0, item.MaxHealth);
    }

    void UpdateAcquired()
    {
        if (acquired)
        {
            Game.Player.Acquire(this);
        }
        else
        {
            Game.Player.Unacquire(this);
        }
    }

    void UpdateItem()
    {
        if(item != null)
        {
            this.gameObject.name = item.name;
        }
    }
}
