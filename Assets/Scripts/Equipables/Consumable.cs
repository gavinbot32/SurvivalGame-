using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Consumable : MonoBehaviour
{
    public UnityEvent ev_consume;
    private EquipableHandler h_equip;

    private void Awake()
    {
        h_equip = GetComponent<EquipableHandler>();
    }

    private void Update()
    {
          

        if(h_equip.invItem == null)
        {
            Destroy(gameObject);
            PlayerController.instance.UpdateToolSlots();

        }

    }

    public void InvokeEvent()
    {
       
        ev_consume.Invoke();
        RemoveFromInv();
       
    }

    private void RemoveFromInv()
    {
        if (h_equip.invItem.itemData.stackable)
        {
            h_equip.invItem.RemoveFromStack(1);
            if(h_equip.invItem.stackSize <= 0)
            {
                PlayerController.instance.UpdateToolSlots();
                Destroy(gameObject);
            }
        }
        else
        {
            h_equip.invItem.DeleteSelf();
            PlayerController.instance.UpdateToolSlots();
            Destroy(gameObject);

        }
        PlayerController.instance.UpdateToolSlots();

    }
}
