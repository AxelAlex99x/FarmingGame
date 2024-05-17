using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment")]
public class EquipmentData : Itemdata
{
    public enum ToolType
    {
         Hoe, Axe, Pickaxe, WateringCan, Shovel
    }
    public ToolType toolType;
}
