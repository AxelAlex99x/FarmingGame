using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public int id;
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }

    public LandStatus lstatus;

    public Material soilMat, farmLandMat, wateredMat;

    new Renderer renderer;

    public GameObject select;

    GameTimeStamp timeWatered;
    GameTimeStamp timeFarmland;

    [Header("Crops")]
    public GameObject cropPrefab;

    CropBehaviour cropPlanted = null;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        SwitchLandStatus(LandStatus.Soil);

        Select(false);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void LoadLandData(LandStatus statusToSwitch, GameTimeStamp lastWatered)
    {
        lstatus = statusToSwitch;
        timeWatered = lastWatered;


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
                break;
        }


        renderer.material = materialToSwitch;
     
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
                timeFarmland = TimeManager.Instance.GetGameTimestamp();
                break;

            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                break;
        }


        renderer.material = materialToSwitch;

        LandManager.Instance.OnLandStateChange(id, lstatus, timeWatered);
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    public void Interact()
    {
        Itemdata toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tools);

        if(!InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Tools)) 
        {
            return;
        }

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
                case EquipmentData.ToolType.Hoe:
                    if(cropPlanted != null)
                    {
                        cropPlanted.RemoveCrop();
                    }
                    break;
            }
            return;
        }

        SeedData seedTool = toolSlot as SeedData;

        if(seedTool != null && lstatus != LandStatus.Soil && cropPlanted == null)
        {
            SpawnCrop();

            cropPlanted.Plant(id, seedTool);

            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tools));
        }
    }

    public CropBehaviour SpawnCrop()
    {
        GameObject cropObject = Instantiate(cropPrefab, transform);

        cropObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        cropPlanted = cropObject.GetComponent<CropBehaviour>();

        return cropPlanted;
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        if(lstatus == LandStatus.Watered)
        {
            int hoursElapse = GameTimeStamp.CompareTimeStamps(timeWatered,timestamp);

            if(cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            if(hoursElapse > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }

        if(lstatus != LandStatus.Watered && cropPlanted != null)
        {
            if(cropPlanted.cropState != CropBehaviour.CropState.Seed)
            {
                cropPlanted.Wither();              
            }
        }
        
        /*if(lstatus == LandStatus.Farmland)
        {
            int hoursElapse = GameTimeStamp.CompareTimeStamps(timeFarmland, timestamp);
            if(hoursElapse > 72 && (!cropPlanted || cropPlanted.cropState == CropBehaviour.CropState.Wilted)               )
            {
                SwitchLandStatus(LandStatus.Soil);
            }
        }*/
    }

    private void OnDestroy()
    {
        TimeManager.Instance.UnregisteredTracker(this);
    }
}