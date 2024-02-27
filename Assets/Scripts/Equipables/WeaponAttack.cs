using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponAttack : MonoBehaviour
{
    private Collider col;
    private Animator anim;
    [SerializeField] private float cooldownDur;
    private float cooldown;
    private void Awake()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        col.enabled = false;
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (cooldown <= 0)
        {
            cooldown = cooldownDur;
            StartCoroutine(attackRoutine());
            
        }
    }

    IEnumerator attackRoutine()
    {
        col.enabled = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        col.enabled = false;
    }

}
