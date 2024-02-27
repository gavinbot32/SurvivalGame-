using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour 
{
    [SerializeField]private int damage;


   

    private void OnCollisionEnter(Collision collision)
    {
        print("collide");
        if(collision.gameObject == this.gameObject)
        {
            return;
        }

        if(collision.gameObject.TryGetComponent(out HeathHandler h_health))
        {
            h_health.TakeDamage(damage);

        }
    }
    private void OnTriggerEnter(Collider collision)
    {
       
        print("collide");
        if (collision.gameObject == this.gameObject)
        {
            return;
        }

        if (collision.gameObject.TryGetComponent(out HeathHandler h_health))
        {
            
           
            h_health.TakeDamage(damage);
           
        }
        
    }
    
}
