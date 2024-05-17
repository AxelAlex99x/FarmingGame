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
    public Itemdata[] tools= new Itemdata[8];
    public Itemdata eqquipedtool= null;
    [Header("Items")]
    public Itemdata[] items= new Itemdata[8];
    public Itemdata equippeditem= null;


    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Items)
        {
            Itemdata itemToEquip = InventoryManager.Instance.items[slotIndex];
            items[slotIndex] = equippeditem;
            equippeditem = itemToEquip;
        }
        else
        {
            Itemdata toolToEquip = InventoryManager.Instance.tools[slotIndex];
            tools[slotIndex] = eqquipedtool;
            eqquipedtool = toolToEquip;
        }
        UIManager.Instance.RenderInventory();
    }

    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Items)
        {
            for(int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = equippeditem;
                    equippeditem = null;
                    break;
                }
            }
            
        }
        else
        {
            for (int i = 0; i < tools.Length; i++)
            {
                if (tools[i] == null)
                {
                    tools[i] = eqquipedtool;
                    eqquipedtool = null;
                    break;
                }
            }
        }
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
