using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CropBehaviour;

[System.Serializable]
public struct CropSaveState 
{
    public int landID;

    public string seedToGrow;
    public CropBehaviour.CropState cropState;
    public int growth;
    public int health;

    public CropSaveState(int landID, string seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        this.health = health;
    }

    public void Grow()
    {
        growth++;

        SeedData seedInfo = (SeedData) InventoryManager.Instance.itemIndex.GetItemFromString(seedToGrow);
        int maxGrowth = GameTimeStamp.HoursToMinute(GameTimeStamp.DaysToHours(seedInfo.DaysToGrow));
        int maxHealth = GameTimeStamp.HoursToMinute(48);
        

        if (health < maxHealth)
        {
            health++;
        }

        if (growth >= maxGrowth / 2 && cropState == CropBehaviour.CropState.Seed)
        {
           cropState = CropBehaviour.CropState.Seedling;
        }

        if (growth >= maxGrowth && cropState == CropBehaviour.CropState.Seedling)
        {
            cropState = CropBehaviour.CropState.Harvestable;
            return;
        }   
    }

    public void Wither()
    {
        health -= 1;

        if (health <= 0 && cropState != CropBehaviour.CropState.Seed)
        {
            cropState = CropBehaviour.CropState.Wilted;
        }
    }
}
