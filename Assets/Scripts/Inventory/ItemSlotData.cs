using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotData
{
    public Itemdata itemData;
    public int quantity;

    public ItemSlotData(Itemdata itemData , int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        ValidateQuantity();
    }

    public ItemSlotData(Itemdata itemData)
    {
        this.itemData= itemData;
        quantity = 1;
        ValidateQuantity();
    }

    public void AddQuantity()
    {
        AddQuantity(1);
    }
    public void AddQuantity(int amountToAdd)
    {
        quantity += amountToAdd;
    }

    public void Remove()
    {
        quantity -= 1;
        ValidateQuantity();
    }
    public void ValidateQuantity() 
    {
        if (quantity <= 0 || itemData == null)
        {
            Empty();
        }
    }
    public void Empty()
    {
        itemData = null;
        quantity = 0;
    }
}
