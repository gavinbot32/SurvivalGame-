using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{

    [SerializeField] private AiComponent[] aiBehaviors;

    private void FixedUpdate()
    {
        AiComponent priority = null;
        foreach (AiComponent aiComponent in aiBehaviors)
        {
            if (aiComponent.ready)
            {
                priority = aiComponent;
                priority.active = true;
                break;
            }
        }

        if (priority)
        {
            foreach (AiComponent aiComponent in aiBehaviors)
            {
                if (aiComponent != priority)
                {
                    aiComponent.active = false;
                }
            }
        }

    }
}
