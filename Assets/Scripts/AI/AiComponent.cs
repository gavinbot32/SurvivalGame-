using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiComponent : MonoBehaviour
{
    public bool active;
    public bool ready;
    public NavMeshAgent agent;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

}
