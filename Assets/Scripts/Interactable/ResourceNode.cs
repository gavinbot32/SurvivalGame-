using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [Header("Components")]
    private HeathHandler h_health;
    private Animator anim;
    private Rigidbody rig;
    [Header("Resource Vars")]
    [SerializeField] Item itemData;

    private void Awake()
    {
        h_health = GetComponent<HeathHandler>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        if (rig)
        {
            rig.isKinematic = true;
        }
    }

    public void HitNode()
    {
        if (PlayerController.instance.inv.OpenSlots())
        {
            PlayerController.instance.inv.AddItem(itemData);
            return;
        }
        else
        {
            if (itemData.stackable)
            {
                if (PlayerController.instance.inv.OpenStack(itemData))
                {
                    PlayerController.instance.inv.AddItem(itemData);
                    return;
                }
            }
        }
    }

    public void AnimDeath()
    {
        anim.SetTrigger("Death");
    }

    public void RigDeath() { 
        rig.isKinematic = false;
        rig.AddForce(new Vector3(0f, 2f, 0f), ForceMode.Impulse);
        GetComponent<Collider>().enabled = false;
    }


}
