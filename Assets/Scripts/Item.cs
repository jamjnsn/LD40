using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 1)]
public class Item : ScriptableObject {
    public Item Replaces;

    public int MaxHealth;
    public int Price;
    public int HappinessPerDay;
    public float NoveltyLossPerDay;
    public float RepairCostPercentage;
    public int DamagePerDay;
    public Sprite Sprite;
    public string Description;
    
    /// <summary>
    /// Cool things are above and beyond the necessities.
    /// </summary>
    public bool IsCool;
}
