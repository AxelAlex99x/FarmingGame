using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{

    SeedData seedToGrow;

    [Header("Stage of Life")]
    public GameObject seed;
    public GameObject wilted;
    private GameObject seedling;
    private GameObject harvestable;

    int growth;
    int maxGrowth;

    int maxHealth = GameTimeStamp.HoursToMinute(48);
    int health;
    public enum CropState
    {
        Seed, Seedling, Harvestable, Wilted
    }

    public CropState cropState;
    public void Plant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;

        seedling = Instantiate(seedToGrow.seedling, transform);

        Itemdata cropToYield = seedToGrow.cropToYield;

        harvestable = Instantiate(cropToYield.gameModel, transform);

        int hoursToGrow = GameTimeStamp.DaysToHours(seedToGrow.DaysToGrow);
        maxGrowth = GameTimeStamp.HoursToMinute(hoursToGrow);

        if (seedToGrow.regrowable)
        {
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();

            regrowableHarvest.SetParent(this);
        }

        SwitchState(CropState.Seed);
    }

    public void Grow()
    {
        growth++;

        if(health < maxHealth)
        {
            health++;
        }

        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        if(growth >= maxGrowth)
        {
            SwitchState(CropState.Harvestable);
        }
    }

    public void Wither()
    {
        health -= 1;
        
        if (health <= 0 && cropState != CropState.Seed)
        {      
            SwitchState(CropState.Wilted);
        }
    }

    void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);
                health = maxHealth;
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);

                if (!seedToGrow.regrowable)
                {
                    harvestable.transform.parent = null;
                    Destroy(gameObject);
                }
                
                break;
            case CropState.Wilted:
                wilted.SetActive(true);
       
                break;
        }
        cropState = stateToSwitch;
    }

    public void Regrow()
    {
        int hoursToRegrow = GameTimeStamp.DaysToHours(seedToGrow.daysToRegrow);
        growth = maxGrowth - GameTimeStamp.HoursToMinute(hoursToRegrow);
        
        SwitchState(CropState.Seedling);

    }
}
