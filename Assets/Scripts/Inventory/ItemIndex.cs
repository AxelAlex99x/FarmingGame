using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Index")]
public class ItemIndex : ScriptableObject
{
    public List<Itemdata> items;

    public Itemdata GetItemFromString(string name)
    {
        return items.Find(i => i.name == name);
    }
}
