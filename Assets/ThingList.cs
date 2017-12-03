using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ThingList : List<Thing> {
    public Thing Find(Item item)
    {
        return this.FirstOrDefault(thing => thing.Item == item);
    }

    public Thing FindByName(string name)
    {
        return this.FirstOrDefault(thing => thing.Item.name == name);
    }
}
