using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }

    public LandStatus lstatus;

    public Material soilMat, farmLandMat, wateredMat;

    new Renderer renderer;

    public GameObject select;

    GameTimeStamp timeWatered;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        SwitchLandStatus(LandStatus.Soil);

        Select(false);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        lstatus = statusToSwitch;
        Material materialToSwitch = soilMat;


        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;

            case LandStatus.Farmland:
                materialToSwitch = farmLandMat;
                break;

            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                break;
        }


        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    public void Interact()
    {
        Itemdata toolSlot = InventoryManager.Instance.eqquipedtool;
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        if (equipmentTool != null) 
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.Shovel:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    if(lstatus == LandStatus.Farmland)
                    SwitchLandStatus(LandStatus.Watered);
                    break;
            }
        }
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        if(lstatus == LandStatus.Watered)
        {
            int hoursElapse = GameTimeStamp.CompareTimeStamps(timeWatered,timestamp);
            if(hoursElapse > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
    }
}
