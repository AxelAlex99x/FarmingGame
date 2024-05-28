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

    public enum FarmObstacleStatus
    {
        None, Rock, Wood, Weeds
    }

    [Header("Obstacles")]
    public FarmObstacleStatus obstacleStatus;
    public GameObject rockPrefab, woodPrefab, weedPrefab;

    GameObject obstacleObject;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        SwitchLandStatus(LandStatus.Soil);

        Select(false);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void LoadLandData(LandStatus LandStatusToSwitch, GameTimeStamp lastWatered, FarmObstacleStatus obstacleStatusToSwitch)
    {
        lstatus = LandStatusToSwitch;
        timeWatered = lastWatered;


        Material materialToSwitch = soilMat;


        switch (LandStatusToSwitch)
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

        switch (obstacleStatusToSwitch)
        {
            case FarmObstacleStatus.None:
                if (obstacleObject != null)
                {
                    Destroy(obstacleObject);
                }
                break;

            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;

            case FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;

            case FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedPrefab, transform);
                break;
        }

        if (obstacleObject != null)
        {
            obstacleObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        obstacleStatus = obstacleStatusToSwitch;

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

        LandManager.Instance.OnLandStateChange(id, lstatus, timeWatered, obstacleStatus);
    }

    public void SetObstacleStatus(FarmObstacleStatus statusToSwitch) 
    {
        switch(statusToSwitch)
        {
            case FarmObstacleStatus.None: 
                if(obstacleObject != null)
                {
                    Destroy(obstacleObject);
                }
                break;

            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;

            case FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;

            case FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedPrefab, transform);
                break;
        }

        if(obstacleObject != null)
        {
            obstacleObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        obstacleStatus = statusToSwitch;

        LandManager.Instance.OnLandStateChange(id, lstatus, timeWatered, obstacleStatus);
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
                    Music.Instance.PlaySfx1();
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    if(lstatus == LandStatus.Farmland)
                    {
                        Music.Instance.PlaySfx2();
                        SwitchLandStatus(LandStatus.Watered);
                    }
                    break;
                case EquipmentData.ToolType.Hoe:
                    if(cropPlanted != null)
                    {
                        cropPlanted.RemoveCrop();
                    }

                    if(obstacleStatus == FarmObstacleStatus.Weeds)
                    {
                        SetObstacleStatus(FarmObstacleStatus.None);
                    }
                    break;

                case EquipmentData.ToolType.Axe:
                    if (obstacleStatus == FarmObstacleStatus.Wood)
                    {
                        SetObstacleStatus(FarmObstacleStatus.None);
                    }
                    break;
                case EquipmentData.ToolType.Pickaxe:
                    if (obstacleStatus == FarmObstacleStatus.Rock)
                    {
                        SetObstacleStatus(FarmObstacleStatus.None);
                    }
                    break;
            }
            return;
        }

        SeedData seedTool = toolSlot as SeedData;

        if(seedTool != null && lstatus != LandStatus.Soil && cropPlanted == null && obstacleStatus == FarmObstacleStatus.None)
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