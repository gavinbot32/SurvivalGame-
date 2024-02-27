using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour, IDropHandler
{
    public Inventory ownInventory;

    private void Awake()
    {
       
    }

    private void Start()
    {
        if (gameObject.CompareTag("ToolSlot"))
        {
            ownInventory = PlayerController.instance.GetComponent<Inventory>();
        }
    }



    public GameObject item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public InventoryItem itemInSlot
    {
        get
        {
            if (item)
            {
                return item.GetComponent<InventoryItem>();
            }
            return null;
        }
    }

    private void Update()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (gameObject.CompareTag("ToolSlot") && DragDrop.itemBeingDragged.GetComponent<InventoryItem>().itemData.equipable == false)
        {
            return;
        }

        if (!item && DragDrop.itemBeingDragged != null)
        {

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);
            if (ownInventory.inventoryItems.Contains(itemInSlot) == false)
            {
                ownInventory.TransferItem(itemInSlot, itemInSlot.currentInv);
            }
        }
        else if (item && DragDrop.itemBeingDragged != null)
        {
            InventoryItem itemDragged = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();
            if (itemDragged.itemData == itemInSlot.itemData && itemInSlot.itemData.stackable)
            {
                itemInSlot.CombineStack(itemDragged);
            }


        }
        if (!itemInSlot)
        {
            return;
        }
        PlayerController.instance.UpdateToolSlots();

    }
}
