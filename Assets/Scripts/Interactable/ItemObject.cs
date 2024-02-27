using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
 
    public Item itemData;

    private void Awake()
    {
    }

    public void pickUp()
    {
        if (PlayerController.instance.inv.OpenSlots())
        {
            PlayerController.instance.inv.AddItem(itemData);
            Destroy(gameObject);
            return;
        }
        else
        {
            if (itemData.stackable)
            {
                if (PlayerController.instance.inv.OpenStack(itemData))
                {
                    PlayerController.instance.inv.AddItem(itemData);
                    Destroy(gameObject);
                    return;
                }
            }
        }
        
    }
}
