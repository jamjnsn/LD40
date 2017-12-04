using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ThingList : List<Thing> {
    public Thing Find(Item item)
    {
        return this.FirstOrDefault(thing => thing.Item == item);
    }

    public Thing FindByName(string name)
    {
        return this.FirstOrDefault(thing => thing.Item != null && thing.Item.name == name);
    }

    public new void Add(Thing thing)
    {
        if(!this.Contains(thing))
        {
            base.Add(thing);
        }
    }

    public new void Remove(Thing thing)
    {
        if(this.Contains(thing))
        {
            base.Remove(thing);
        }
    }
}
