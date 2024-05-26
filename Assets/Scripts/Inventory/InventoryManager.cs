using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }
    }

    [Header("Tools")]
    [SerializeField]
    private ItemSlotData[] toolSlots = new ItemSlotData[8];
    [SerializeField]
    private ItemSlotData equippedToolSlot = null;
    [Header("Items")]
    [SerializeField]
    private ItemSlotData[] itemSlots = new ItemSlotData[8];
    [SerializeField]
    private ItemSlotData equippedItemSlot = null;

    public Transform handPoint;
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handToEquip = equippedToolSlot;
        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InventorySlot.InventoryType.Items)
        {
            handToEquip = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            handToEquip.AddQuantity(slotToAlter.quantity);

            slotToAlter.Empty();


        } else
        {
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            EquipHandSlot(slotToEquip);
        }

        if (inventoryType == InventorySlot.InventoryType.Items)
        {
           // equippedItemSlot = handToEquip;
            RenderHand();
        }
       /* else
        {
            equippedToolSlot = handToEquip;
        }*/

        UIManager.Instance.RenderInventory();
    }

    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = equippedToolSlot;

        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InventorySlot.InventoryType.Items)
        {
            handSlot = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(handSlot);
                    handSlot.Empty();
                    break;
                }
            }
        }

        if (inventoryType == InventorySlot.InventoryType.Items)
        {        
            RenderHand();
        }

        UIManager.Instance.RenderInventory();      
    }

    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for(int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                inventoryArray[i].AddQuantity(itemSlot.quantity);
                itemSlot.Empty();
                return true;
            }
        }
        return false;
    }

    public void RenderHand()
    {
        if(handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        
        if(SlotEquipped(InventorySlot.InventoryType.Items))
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
            return !equippedItemSlot.IsEmpty();
        }
        return !equippedToolSlot.IsEmpty();
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
    public void EquipHandSlot(Itemdata item)
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

    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        Itemdata item = itemSlot.itemData;
        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(itemSlot);
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

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if (itemSlot.IsEmpty())
        {
            Debug.Log("There is nothing to consume");
            return;
        }
        
        itemSlot.Remove();

        RenderHand();

        UIManager.Instance.RenderInventory();
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
