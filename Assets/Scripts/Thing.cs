using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Thing : MonoBehaviour {
    [SerializeField]
    [OnValueChanged("UpdateItem")]
    public Item Item;

    public bool IsItem(Item item)
    {
        return this.Item == item;
    }

    [OnValueChanged("UpdateHealth")]
    public int Health;
    
    [SerializeField]
    [OnValueChanged("UpdateAcquired")]
    bool acquired;
    public bool Acquired
    {
        get
        {
            return acquired;
        }

        set
        {
            acquired = value;
            gameObject.SetActive(value);
        }
    }

    [SerializeField]
    float novelty;
    public float Novelty
    {
        get
        {
            return novelty;
        }

        set
        {
            novelty = Mathf.Clamp(value, -1, 1);
        }
    }

    public int HappinessPerDay
    {
        get
        {
            return Mathf.RoundToInt((float)Item.HappinessPerDay * Novelty);
        }
    }

    public float HealthPercentage
    {
        get
        {
            return Item.MaxHealth == 0 ? 1 : Health / Item.MaxHealth;
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
        Health = Item.MaxHealth;
    }

	// Use this for initialization
	void Start () {
        Acquired = acquired;
        UpdateItem();
        Novelty = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateHealth()
    {
        Health = Mathf.Clamp(Health, 0, Item.MaxHealth);
    }

    void UpdateItem()
    {
        if(Item != null)
        {
            this.gameObject.name = Item.name;
            Health = Item.MaxHealth;
        }
    }

    void UpdateAcquired()
    {
        if (Application.isPlaying)
        {
            if (Acquired)
            {
                Game.Player.Acquire(this);
            }
            else
            {
                Game.Player.Unacquire(this);
            }
        }
    }
}
