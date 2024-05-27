using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance {  get; private set; }

    bool screenFadedOut;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        if(SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm) 
        {
            if(LandManager.farmData == null) { return; }
            
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

            if(cropData.Count == 0)
            {
                return;
            }

            for(int i = 0; i < cropData.Count; i++)
            {
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landID];

                if(crop.cropState == CropBehaviour.CropState.Wilted)
                {
                    continue;
                }

                land.ClockUpdate(timestamp);

                if(land.landStatus == Land.LandStatus.Watered)
                {
                    crop.Grow();
                }
                else if(crop.cropState != CropBehaviour.CropState.Seed)
                {
                    crop.Wither();
                }

                cropData[i] = crop;
                landData[crop.landID] = land;

            }

            LandManager.farmData.Item2.ForEach((CropSaveState crop) =>
            {
                Debug.Log(crop.seedToGrow + "\n Health: " + crop.health + "\n Growth: " + crop.growth + "\n State: "
                    + crop.cropState.ToString());
            });
        }
    }

    public void Sleep()
    {
        UIManager.Instance.FadeOutScreen();
        screenFadedOut = false;
        StartCoroutine(TransitionTime());
    }

    IEnumerator TransitionTime()
    {
        // Debug.Log("Sleep");
        GameTimeStamp timestampOfNextDay = TimeManager.Instance.GetGameTimestamp();
        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;
        Debug.Log(timestampOfNextDay.day + " " + timestampOfNextDay.hour + ":" + timestampOfNextDay.minute);

        while (!screenFadedOut)
        {
            yield return new WaitForSeconds(1f);
        }

        TimeManager.Instance.SkipTime(timestampOfNextDay);

        SaveManager.Save(ExportSaveState());

        screenFadedOut = false;

        UIManager.Instance.ResetFadeDefaults();
    }

    public void OnFadeOutComplete()
    {
        screenFadedOut = true;
    }

    public GameSaveState ExportSaveState()
    {
        List<LandSaveState> landData = LandManager.farmData.Item1;
        List<CropSaveState> cropData = LandManager.farmData.Item2;

        ItemSlotData[] toolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tools);
        ItemSlotData[] itemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Items);

        ItemSlotData equippedToolSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tools);
        ItemSlotData equippedItemSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Items);

        GameTimeStamp timestamp = TimeManager.Instance.GetGameTimestamp();

        return new GameSaveState(landData, cropData, toolSlots, itemSlots, equippedItemSlot, equippedToolSlot, timestamp);
    }

    public void LoadSave()
    {
        GameSaveState save = SaveManager.Load(); 

        

        TimeManager.Instance.LoadTime(save.timestamp);

        ItemSlotData[] toolSlots = save.toolSlots;
        ItemSlotData equippedToolSlot = save.equippedToolSlot;
        ItemSlotData[] itemSlots = save.itemSlots;
        ItemSlotData equippedItemSlot = save.equippedItemSlot;

        InventoryManager.Instance.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);

        LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(save.landData, save.cropData);
    }
}
