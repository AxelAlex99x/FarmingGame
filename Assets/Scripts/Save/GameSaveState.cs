using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState 
{
    public List<LandSaveState> landData;
    public List<CropSaveState> cropData;

    public ItemSlotData[] toolSlots;
    public ItemSlotData[] itemSlots;

    public ItemSlotData equippedItemSlot;
    public ItemSlotData equippedToolSlot;

    public GameTimeStamp timestamp;

    public GameSaveState(List<LandSaveState> landData, List<CropSaveState> cropData, ItemSlotData[] toolSlots, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot, ItemSlotData equippedToolSlot, GameTimeStamp timestamp)
    {
        this.landData = landData;
        this.cropData = cropData;
        this.toolSlots = toolSlots;
        this.itemSlots = itemSlots;
        this.equippedItemSlot = equippedItemSlot;
        this.equippedToolSlot = equippedToolSlot;
        this.timestamp = timestamp;
    }
}
