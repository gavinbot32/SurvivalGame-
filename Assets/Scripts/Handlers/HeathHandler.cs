using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeathHandler : MonoBehaviour
{
    public int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private float dealthDelay;
    public UnityEvent ev_damage;
    public UnityEvent ev_death;

    private void Awake()
    {
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(ev_damage != null)
        {
            ev_damage.Invoke();
        }
        DeathCheck();
    }

    public void GainHealth(int amount)
    {
        health += amount;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void DeathCheck()
    {
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if(ev_death != null)
        {
            ev_death.Invoke();
        }
        Destroy(gameObject, dealthDelay);

    }



}
