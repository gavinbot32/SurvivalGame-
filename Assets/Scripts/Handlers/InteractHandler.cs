using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    [SerializeField] private float interactableRange;
    float interactCooldown;

    [SerializeField] public Interactable interactObject;


    private void Update()
    {
        if (interactCooldown > 0)
        {
            interactCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //closestInteractable();
        interactObject = castedInteractale();
    }
    private void closestInteractable()
    {
        Interactable[] interactables = FindObjectsOfType<Interactable>();
        Interactable closest = interactables[0];
        foreach (Interactable inter in interactables)
        {
            if (Vector3.Distance(transform.position, inter.transform.position) < Vector3.Distance(transform.position, closest.transform.position))
            {
                closest = inter;
            }
        }
        if (Vector3.Distance(transform.position, closest.transform.position) <= interactableRange)
        {
            interactObject = closest;
        }
        else
        {
            interactObject = null;
        }
    }

    private Interactable castedInteractale()
    {
        Interactable ret = null;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(ray,out RaycastHit hit, interactableRange)){

            if(hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interact))
            {
                ret = interact;
                
            }

        }
        return ret;
    }
    public void HandleInteract()
    {
        if (interactObject != null)
        {
            if (interactCooldown <= 0)
            {
                interactCooldown = 1;
                interactObject.interact();
            }
        }
    }

}
