using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Seed")]
public class SeedData : Itemdata
{
    public int DaysToGrow;

    public Itemdata cropToYield;

    public GameObject seedling;
}
