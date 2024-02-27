using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{

    [SerializeField]private GameObject prefab;
    [SerializeField]private GameObject phantomPrefab;
    private GameObject phantom_obj;
    [SerializeField]private LayerMask build_layer;
    [SerializeField] private Material phantomMat;
    [SerializeField] private Material redPhantomMat;
    bool valid;

    // Start is called before the first frame update
    void Start()
    {
        valid = true;
        phantom_obj = Instantiate(phantomPrefab);
        phantomMat = phantom_obj.GetComponent<MeshRenderer>().material;
    }
    private void OnDestroy()
    {
        Destroy(phantom_obj);
    }
    private void OnDisable()
    {
        Destroy(phantom_obj);
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 10f, build_layer))
        {  
            phantom_obj.transform.position = new Vector3(hit.point.x, hit.point.y+phantom_obj.transform.localScale.y/2,hit.point.z);
            valid = true;
        }
        else if (Physics.Raycast(ray, out hit, 10f))
        {
            phantom_obj.transform.position = new Vector3(hit.point.x, hit.point.y + phantom_obj.transform.localScale.y / 2, hit.point.z);
            valid = false;

        }
        else
        {
            phantom_obj.transform.position = new Vector3(ray.GetPoint(10f).x, ray.GetPoint(10f).y + phantom_obj.transform.localScale.y / 2, ray.GetPoint(10f).z);
            valid = false;
        }


    }


    private bool FloorCheck()
    {
        bool ret;
        float height = (phantom_obj.transform.position.y - (phantom_obj.transform.localScale.y / 2))+0.1f;
        float posX = phantom_obj.transform.position.x;
        float posY = phantom_obj.transform.position.y;
        float posZ = phantom_obj.transform.position.z;
        float scaX = phantom_obj.transform.localScale.x;
        float scaY = phantom_obj.transform.localScale.y;
        float scaZ = phantom_obj.transform.localScale.z;

        Ray corner_A = new Ray(new Vector3(posX - scaX / 2, height, posZ - scaZ/2), Vector3.down);
        Ray corner_B = new Ray(new Vector3(posX + scaX / 2, height, posZ + scaZ/2), Vector3.down);
        Ray corner_C = new Ray(new Vector3(posX - scaX / 2, height, posZ + scaZ/2), Vector3.down);
        Ray corner_D = new Ray(new Vector3(posX + scaX / 2, height, posZ - scaZ/2), Vector3.down);
        
        // debug
/*
        Debug.DrawRay(new Vector3(posX - scaX / 2, height, posZ - scaZ / 2), Vector3.down, Color.blue, 1f);
        Debug.DrawRay(new Vector3(posX + scaX / 2, height, posZ + scaZ / 2), Vector3.down, Color.blue, 1f);
        Debug.DrawRay(new Vector3(posX - scaX / 2, height, posZ + scaZ / 2), Vector3.down, Color.blue, 1f);
        Debug.DrawRay(new Vector3(posX + scaX / 2, height, posZ - scaZ / 2), Vector3.down, Color.blue, 1f);
*/
        Ray[] cornerList = new Ray[4] {corner_A,corner_B,corner_C,corner_D};

        ret = true;
        foreach(Ray ray in cornerList)
        {
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 0.15f, build_layer))
            {
                ret = false;
                return ret;
            }
            
        }
        return ret;
    }
    private void FixedUpdate()
    {
        

        valid = FloorCheck();

        foreach (MeshRenderer mr in phantom_obj.GetComponentsInChildren<MeshRenderer>())
        {
            if (valid)
            {
                mr.material = phantomMat;
            }
            else
            {
                mr.material = redPhantomMat;
            }
        }
    }

    public void build()
    {
        if (valid)
        {
            Instantiate(prefab, phantom_obj.transform.position, Quaternion.identity);
            InventoryItem invItem = GetComponent<EquipableHandler>().invItem;
            invItem.DeleteSelf();
            Destroy(gameObject);
        }
    }
}
