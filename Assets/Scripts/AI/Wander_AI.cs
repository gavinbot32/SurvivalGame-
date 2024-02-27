using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander_AI : AiComponent
{

    [SerializeField] private Vector2 mm_idleTimer;
    [SerializeField] private Vector2 mm_wanderTime;
    [SerializeField] private Vector2 mm_range;
    private bool wandering;

    public float idleTimer;
    public float wanderTime;
    public Vector3 targetPos;



    private void Start()
    {
        
    }

    private void Update()
    {
        if (!active) return;
        idleTimer = timerSet(idleTimer);
        wanderTime = timerSet(wanderTime);
        if((wandering && wanderTime <= 0) || (!wandering && idleTimer <= 0))
        {
            SwitchStates();
        }

        if(targetPos != Vector3.zero)
        {
            if(transform.position == targetPos)
            {
                targetPos = new Vector3(Random.Range(mm_range.x, mm_range.y), transform.position.y, Random.Range(mm_range.x, mm_range.y));
                agent.SetDestination(targetPos);
            }
        }

    }


    public void SwitchStates()
    {
        if(Random.Range(0,2) == 0) wandering = true;
        else wandering = false;
        if(wandering )
        {
            agent.isStopped = false;
            wanderTime = Random.Range(mm_wanderTime.x,mm_wanderTime.y);
            targetPos = new Vector3(Random.Range(mm_range.x, mm_range.y), transform.position.y, Random.Range(mm_range.x, mm_range.y));
            agent.SetDestination(targetPos);
            
            
        }
        else
        {
            
            idleTimer = Random.Range(mm_idleTimer.x,mm_idleTimer.y);
            agent.isStopped = true;
            targetPos = Vector3.zero;
        }

    }


    private float timerSet(float timer)
    {
        float ret = timer;
        if(timer > 0)
        {
            ret -= Time.deltaTime;
            return ret;
        }
        return 0;

    }

}
