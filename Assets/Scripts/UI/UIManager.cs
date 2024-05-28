using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }

    [Header("Status Bar")]
    public Image toolEquipedSlot;
    public TextMeshProUGUI toolQuantityText;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    [Header("Inventory System")]
    public GameObject inventoryPanel;
    public HandInventorySlot toolHandSlot;

    public InventorySlot[] toolSlots;

    public HandInventorySlot itemHandSlot;
    public InventorySlot[] itemSlots;

    [Header("Item Info Box")]
    public GameObject itemInfoBox;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    [Header("Screen Transitions")]
    public GameObject fadeIn;
    public GameObject fadeOut;


    [Header("Yes No Prompt")]
    public YesNoPrompt yesNoPrompt;

    [Header("Player Stats")]
    public TextMeshProUGUI moneyText;

    [Header("Shop")]
    public ShopListingManager shopListingManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        RenderInventory();
        AssignSlotIndexes();
        RenderPlayerStats();
        DisplayItemInfo(null);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void TriggerYesNoPrompt(string message, System.Action onYesCallBack)
    {
        yesNoPrompt.gameObject.SetActive(true);

        yesNoPrompt.CreatePrompt(message, onYesCallBack);
    }

    public void FadeOutScreen()
    {
        fadeOut.SetActive(true);
    }

    public void FadeInScreen()
    {
        fadeIn.SetActive(true);
    }
    public void OnFadeInComplete()
    {
        fadeIn.SetActive(false);
    }

    public void ResetFadeDefaults()
    {
        fadeOut.SetActive(false);
        fadeIn.SetActive(true);
    }

    public void AssignSlotIndexes()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        ItemSlotData[] inventoryToolSlots= InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tools);
        ItemSlotData[] inventoryItemSlots= InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Items);

        RenderInventoryPanel(inventoryToolSlots,toolSlots);
        RenderInventoryPanel(inventoryItemSlots,itemSlots);

        toolHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tools));
        itemHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Items));

        Itemdata equippedTool= InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tools);

        toolQuantityText.text = "";

        if (equippedTool != null)
        {
            toolEquipedSlot.sprite = equippedTool.thumbnail;
            toolEquipedSlot.gameObject.SetActive(true);

            int quantity = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tools).quantity;

            if(quantity > 1)
            {
                toolQuantityText.text = quantity.ToString();
            }
            return;
        }

        toolEquipedSlot.gameObject.SetActive(false);

    }

    void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
    {
        for(int i = 0;i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }
    }

    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        RenderInventory();
    }

    public void DisplayItemInfo(Itemdata data)
    {
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemInfoBox.SetActive(false);
            return;
        }

        itemInfoBox.SetActive(true);       
        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;
        string prefix = "AM ";
        if(hours > 12)
        {
            prefix = "PM ";
            hours -= 12;
        }
        timeText.text = prefix + hours + ":" + minutes.ToString("00");

        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season + " " + day + " (" + dayOfTheWeek + ")";
    }

    public void RenderPlayerStats()
    {
        moneyText.text = PlayerStats.Money + PlayerStats.CURRENCY;
    }

    public void OpenShop(List<Itemdata> shopItems)
    {
        shopListingManager.gameObject.SetActive(true);
        shopListingManager.RenderShop(shopItems);
    }
}
