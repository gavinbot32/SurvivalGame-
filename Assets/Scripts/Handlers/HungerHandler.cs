using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HungerHandler : MonoBehaviour
{
    public int hunger;
    [SerializeField] int hungerMax;

    [Header("Components")]
    private HeathHandler h_health;


    [Header("Cooldowns")]
    private float healCooldown;
    [SerializeField] private float healDur;
    public float stamTaken;

    private float dmgCooldown;
    private float dmgCooldownDur;

    private void Awake()
    {
        h_health = GetComponent<HeathHandler>();
        dmgCooldownDur = 5;
    }

    private void Update()
    {
        if(healCooldown > 0)
        {
            healCooldown -= Time.deltaTime;
        }
        if(dmgCooldown > 0)
        {
            dmgCooldown -= Time.deltaTime;
        }
        if (hunger > 0)
        {
            if (stamTaken > 100)
            {
                stamTaken = 0;
                hunger -= 1;
            }
        }

        if(hunger <= 0)
        {
            if (dmgCooldown <= 0)
            {
                dmgCooldown = dmgCooldownDur;
                h_health.TakeDamage(5);
            }
        }



        heal();
    }

    public void heal()
    {
        if(hunger >= 90 && h_health.health < 100)
        {
            if (healCooldown <= 0)
            {
                healCooldown = healDur;
                h_health.GainHealth(5);
                hunger -= 1;
            }
        }
    }

    public void GainHunger(int amount)
    {
        hunger += amount;
        if(hunger >= hungerMax)
        {
            hunger = hungerMax;
        }
    }


}
