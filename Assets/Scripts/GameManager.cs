using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    [Header("Inventory Vars")]
    public GameObject invObj;
    public GameObject invContainer;
    public GameObject canvas;



    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void inventoryInitialize()
    {
        InventoryObject[] children = canvas.GetComponentsInChildren<InventoryObject>();
        foreach (InventoryObject child in children)
        {
            if(child.gameObject != PlayerController.instance.GetComponent<Inventory>().invObj)
            {
                child.transform.SetParent(null);
            }
        }

        foreach (InventoryObject child in children)
        {
            child.transform.SetParent(canvas.transform);
        }

    }

}
