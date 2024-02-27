using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipableHandler : MonoBehaviour
{
    public UnityEvent ev_equipable;

    public InventoryItem invItem;

    [field: Header("Leave null for no cooldown/charge time")]
    [Tooltip("You can use cooldown as a charge or a cooldown")]
    [field: SerializeField] public float cooldown { get; private set; }
    [field: SerializeField] public bool isCharge { get; private set; }

    public void EventInvoke()
    {
        ev_equipable.Invoke();
    }
}
