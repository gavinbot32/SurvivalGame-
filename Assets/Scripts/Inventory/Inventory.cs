using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Inventory : MonoBehaviour
{

    public int cellCount;

    public GameObject cellPrefab;

    GameManager manager;
    public GameObject invContainer;
    public GameObject invObj;
    public GameObject invObjPrefab;
    public GameObject inventoryItemPrefab;
    public bool playerInv;
    private UIManager uiManager;

    [SerializeField] List<ItemSlot> itemSlots;

    [SerializeField] Item[] items;

    public List<InventoryItem> inventoryItems;


    

    private void Awake()
    {
        inventoryItems = new List<InventoryItem>();
        manager = FindObjectOfType<GameManager>();
        if (invObj == null)
        {
            invObj = Instantiate(invObjPrefab, manager.canvas.transform);
        }
        invContainer = invObj.GetComponentInChildren<GridLayoutGroup>().gameObject;
        invObj.SetActive(false);
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        initializeInv();
    }

    public void initializeInv()
    {
        for (int i = 0; i < cellCount; i++)
        {
            GameObject itemSlot = Instantiate(cellPrefab, invContainer.transform);
            itemSlot.GetComponent<ItemSlot>().ownInventory = this;
            itemSlots.Add(itemSlot.GetComponent<ItemSlot>());
        }
        manager.inventoryInitialize();
    }

    public void toggleInventory()
    {
        if (invObj.activeInHierarchy)
        {
            uiManager.ScreenOff();
        }
        else
        {
            if (!playerInv)
            {
                GameObject[] screens = new GameObject[] { invObj, PlayerController.instance.GetComponent<Inventory>().invObj };
                uiManager.ScreensOn(screens, true);
            }
            else
            {
                uiManager.ScreenOn(PlayerController.instance.inv.invObj, true);

            }
        }
    }

    public bool OpenSlots()
    {
        bool openSlot = false;
        foreach(ItemSlot itemslot in itemSlots) {
            if (!itemslot.itemInSlot)
            {
                openSlot = true;
                return openSlot;
            }
        }

        return openSlot;
    }
    public bool OpenStack(Item itemData)
    {
        bool openStack = false;
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.itemInSlot)
            {
                if (itemSlot.itemInSlot.itemData == itemData && itemSlot.itemInSlot.stackSize < itemSlot.itemInSlot.stackMax)
                {
                    openStack = true;
                    return openStack;
                }
            }
        }

        return openStack;

    }
    public void AddItem(Item item)
    {
        bool findNewSlot = true;
        if (item.stackable)
        {
            foreach(ItemSlot itemSlot in itemSlots)
            {
                if (itemSlot.itemInSlot)
                {
                    if (itemSlot.itemInSlot.itemData == item && itemSlot.itemInSlot.stackSize < itemSlot.itemInSlot.stackMax)
                    {
                        itemSlot.itemInSlot.AddToStack();
                        findNewSlot = false;
                        break; 
                    }
                }
            }
        }
        if(findNewSlot)
        {
            foreach (ItemSlot itemSlot in itemSlots)
            {
                if (itemSlot.item == null && itemSlot.gameObject.CompareTag("ToolSlot") == false)
                {
                   InventoryItem newItem = Instantiate(inventoryItemPrefab, itemSlot.transform).GetComponent<InventoryItem>();
                    newItem.currentInv = this;
                    newItem.itemData = item;
                    inventoryItems.Add(newItem);
                    break;
                }
            }
        }
    }

    public void TransferItem(InventoryItem item, Inventory prevInv)
    {
        inventoryItems.Add(item);
        prevInv.inventoryItems.Remove(item);
        item.currentInv = this;

    }

    public bool CheckForItem(Item item, int count) {

        List<InventoryItem> checkedItems = new List<InventoryItem>();
        int curCount = 0;
        foreach (InventoryItem invItem in inventoryItems) {
        
            if(checkedItems.Contains(invItem) == false)
            {
                checkedItems.Add(invItem);
                if(invItem.itemData == item)
                {
                    curCount+=invItem.stackSize;
                }
            } 

        }
        return curCount >= count;

    }

    public void RemoveItem(Item item, int amount)
    {
        int amountLeft = amount;
        foreach(InventoryItem invItem in inventoryItems)
        {
            if (amountLeft <= 0) return;
            if(invItem.itemData == item)
            {
                if(invItem.stackSize >= amountLeft)
                {
                    invItem.RemoveFromStack(amountLeft);
                }
                else
                {
                    amountLeft -= invItem.stackSize;
                    invItem.RemoveFromStack(amountLeft);
                }
            }
        }
    }
 
}
