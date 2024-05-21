using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Itemdata item;

    public virtual void Pickup()
    {
        InventoryManager.Instance.equippeditem = item;
        InventoryManager.Instance.RenderHand();
        Destroy(gameObject);
    }
}
