using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting_AI : AiComponent
{

    [SerializeField] private float targetRange;
    [SerializeField] string[] targetTags;
    private List<string> targetTagsList;

    [SerializeField] private Transform muzzle;

    private Transform target;
    public bool targeting;

    private float checkCooldown;
    private bool wasChecked;

    // Start is called before the first frame update
    void Start()
    {
        targetTagsList = new List<string>();
        foreach(string tag in targetTags) { 
        targetTagsList.Add(tag);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        debug();
        if (checkCooldown > 0)
        {
            checkCooldown -= Time.deltaTime;
        }

        if (targeting)
        {
            if (!wasChecked)
            {
                StartCoroutine(TargetRoutine());
            }
        }
        else
        {
            targeting = inSight();
        }

        ready = targeting;


        if (!active) return;
        if(targeting)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.isStopped = true;
            ready = false;
        }

    }

    private bool inSight()
    {
        Ray foward = new Ray(muzzle.position, muzzle.forward);
        Ray down = new Ray(muzzle.position, new Vector3(muzzle.forward.x, muzzle.forward.y - 0.5f, muzzle.forward.z));
        Ray left = new Ray(muzzle.position, new Vector3(muzzle.forward.x-0.5f, muzzle.forward.y, muzzle.forward.z));
        Ray leftDown = new Ray(muzzle.position, new Vector3(muzzle.forward.x-0.5f, muzzle.forward.y-0.5f, muzzle.forward.z));
        Ray right = new Ray(muzzle.position, new Vector3(muzzle.forward.x+0.5f, muzzle.forward.y, muzzle.forward.z));
        Ray rightDown = new Ray(muzzle.position, new Vector3(muzzle.forward.x+0.5f, muzzle.forward.y-0.5f, muzzle.forward.z));

        Ray[] rays = new Ray[6] {foward, down, left, right, rightDown, leftDown };

        foreach(Ray ray in rays)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, targetRange))
            {
                if (targetTagsList.Contains(hit.collider.gameObject.tag))
                {
                    target = hit.collider.transform;
                    return true;
                }
            }
        }
        return false;

    }

    private bool inSphere()
    {
        if(Physics.CheckSphere(transform.position, targetRange, 7))
        {
            return true;
        }
        return false;
    }

    private void debug()
    {
        Debug.DrawRay(muzzle.position, muzzle.forward, Color.blue);
        Debug.DrawRay(muzzle.position, new Vector3(muzzle.forward.x, muzzle.forward.y - 0.5f, muzzle.forward.z), Color.blue);
        Debug.DrawRay(muzzle.position, new Vector3(muzzle.forward.x-0.5f, muzzle.forward.y - 0.5f, muzzle.forward.z), Color.blue);
        Debug.DrawRay(muzzle.position, new Vector3(muzzle.forward.x-0.5f, muzzle.forward.y, muzzle.forward.z), Color.blue);
        Debug.DrawRay(muzzle.position, new Vector3(muzzle.forward.x+0.5f, muzzle.forward.y - 0.5f, muzzle.forward.z), Color.blue);
        Debug.DrawRay(muzzle.position, new Vector3(muzzle.forward.x+0.5f, muzzle.forward.y, muzzle.forward.z), Color.blue);

    }

    IEnumerator TargetRoutine()
    {
        print("start of routine");
        wasChecked = true;
        yield return new WaitForSeconds(10f);
        targeting = inSight();
        wasChecked = false;
        print("end of routine");
    }
}
