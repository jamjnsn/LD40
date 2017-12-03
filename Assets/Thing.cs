using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour {
    [SerializeField]
    public Item Item;

    [OnValueChanged("UpdateHealth")]
    public int Health;

    [OnValueChanged("UpdateAcquired")]
    public bool Acquired;

    public float Decay;

    public int HappinessPerDay
    {
        get
        {
            return Mathf.RoundToInt((float)Item.HappinessPerDay * Decay * HealthPercentage);
        }
    }

    public float HealthPercentage
    {
        get
        {
            return Health / Item.MaxHealth;
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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateHealth()
    {
        Health = Mathf.Clamp(Health, 0, Item.MaxHealth);
    }

    void UpdateAcquired()
    {
        if (Acquired)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
