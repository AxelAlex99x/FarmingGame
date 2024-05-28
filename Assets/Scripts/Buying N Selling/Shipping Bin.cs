using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingBin : InteractableObject
{
    public static int hourToShip = 18;
    public static List<ItemSlotData> itemsToShip = new List<ItemSlotData>();

    public override void Pickup()
    {
        Itemdata handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Items);

        if(handSlotItem == null)
        {
            return;
        }

        UIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} ?", PlaceItemsInShippingBin);
    }

    void PlaceItemsInShippingBin()
    {
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Items);

        itemsToShip.Add(new ItemSlotData(handSlot));

        handSlot.Empty();

        InventoryManager.Instance.RenderHand();

        foreach(ItemSlotData item in itemsToShip)
        {
            Debug.Log($"{item.itemData.name} x {item.quantity}");
        }
    }

    public static void ShipItems()
    {
        int moneyToReceive = TallyItems(itemsToShip);

        PlayerStats.Earn(moneyToReceive);

        itemsToShip.Clear();
    }

    static int TallyItems(List<ItemSlotData> items)
    {
        int total = 0;
        foreach(ItemSlotData item in items)
        {
            total += item.quantity * item.itemData.cost;
        }
        return total;
    }
}
