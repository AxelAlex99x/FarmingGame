using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Itemdata item;

    public virtual void Pickup()
    {
        InventoryManager.Instance.EquipHandSlot(item);
        InventoryManager.Instance.RenderHand();
        Destroy(gameObject);
    }
}
