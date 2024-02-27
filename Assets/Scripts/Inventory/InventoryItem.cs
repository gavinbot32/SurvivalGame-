using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Item itemData;
    public int stackSize = 1;
    public int stackMax = 64;
    public Image image;
    public TextMeshProUGUI stackText;

    public Inventory currentInv;

    private void OnEnable()
    {
        updateUI();
    }

    private void Start()
    {
        stackText.gameObject.SetActive(itemData.stackable);
        image.sprite = itemData.sprite;
    }

    public void AddToStack()
    {
        stackSize++;
        updateUI();
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
        updateUI();
    }

    private void Update()
    {
        if(itemData.stackable && stackSize <= 0)
        {
            DeleteSelf();
        }
    }

    public void updateUI()
    {
        stackText.text = stackSize.ToString();

    }

    public void DeleteSelf()
    {
        currentInv.inventoryItems.Remove(this);
        Destroy(gameObject);
    }

    public void CombineStack(InventoryItem otherStack)
    {

        if(otherStack.stackSize + stackSize > stackMax)
        {
            for (int i = 0; i < otherStack.stackMax; i++)
            {
                if(stackSize >= stackMax)
                {
                    break;
                }
                stackSize++;
                otherStack.stackSize--;
            }
        }
        else
        {
            stackSize += otherStack.stackSize;
            otherStack.stackSize = 0;
        }
        updateUI();
        otherStack.updateUI();
    }

}
