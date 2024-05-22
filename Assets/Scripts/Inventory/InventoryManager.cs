using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }else
        {
            Instance = this;
        }
    }

    [Header("Tools")]
    [SerializeField]
    private ItemSlotData[] toolSlots= new ItemSlotData[8];
    [SerializeField]
    private ItemSlotData equippedToolSlot= null;
    [Header("Items")]
    [SerializeField]
    private ItemSlotData[] itemSlots = new ItemSlotData[8];
    [SerializeField]
    private ItemSlotData equippedItemSlot= null;

    public Transform handPoint;
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        /*
        if (inventoryType == InventorySlot.InventoryType.Items)
        {
            Itemdata itemToEquip = InventoryManager.Instance.itemSlots[slotIndex];
            itemSlots[slotIndex] = equippedItemSlot;
            equippedItemSlot = itemToEquip;

            RenderHand();
        }
        else
        {
            Itemdata toolToEquip = InventoryManager.Instance.toolSlots[slotIndex];
            toolSlots[slotIndex] = eqquipedToolSlot;
            eqquipedToolSlot = toolToEquip;
        }
        UIManager.Instance.RenderInventory();*/
    }

    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        /*
        if (inventoryType == InventorySlot.InventoryType.Items)
        {
            for(int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i] == null)
                {
                    itemSlots[i] = equippedItemSlot;
                    equippedItemSlot = null;
                    break;
                }
            }

            RenderHand();
        }
        else
        {
            for (int i = 0; i < toolSlots.Length; i++)
            {
                if (toolSlots[i] == null)
                {
                    toolSlots[i] = eqquipedToolSlot;
                    eqquipedToolSlot = null;
                    break;
                }
            }
        }
        UIManager.Instance.RenderInventory();*/
    }

    public void RenderHand()
    {
        if(handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        
        if(equippedItemSlot != null)
        {
            Instantiate(GetEquippedSlotItem(InventorySlot.InventoryType.Items).gameModel, handPoint);
            GetEquippedSlotItem(InventorySlot.InventoryType.Items).gameModel.transform.localPosition = Vector3.zero;
            GetEquippedSlotItem(InventorySlot.InventoryType.Items).gameModel.transform.localRotation = Quaternion.identity;
        }     
    }


    public Itemdata GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == InventorySlot.InventoryType.Items)
        {
            return equippedItemSlot.itemData;
        }
        return equippedToolSlot.itemData;
    }

    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Items)
        {
            return equippedItemSlot;
        }
        return equippedToolSlot;
    }

    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == InventorySlot.InventoryType.Items)
        {
            return itemSlots;
        }
        return toolSlots;
    }

    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Items)
        {
            return equippedItemSlot != null;
        }
        return equippedToolSlot != null;
    }

    public bool IsTool(Itemdata item)
    {
        EquipmentData equipment = item as EquipmentData;
        if(equipment != null)
        {
            return true;
        }

        SeedData seed = item as SeedData;
        return seed != null;
    }
    public void EquipEmptySlot(Itemdata item)
    {
        if(IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(item);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(item);
        }   
    }

    private void OnValidate()
    {
        ValidateInventorySlot(equippedToolSlot);
        ValidateInventorySlot(equippedItemSlot);

        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);
    }

    void ValidateInventorySlot(ItemSlotData slot)
    {
        if(slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
    }

    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach(ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
