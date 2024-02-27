using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    public UnityEvent interactEvent;
    float interactCooldown;

    private void Awake()
    {
    }

    private void Update()
    {
        if(interactCooldown > 0)
        {
            interactCooldown -= Time.deltaTime;
        }
    }

    public void interact()
    {
        if (interactCooldown <= 0)
        {
            interactCooldown = 1;
            interactEvent.Invoke();
        }
    }




}
