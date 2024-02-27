using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private int food;

    public void Eat()
    {
        PlayerController.instance.GainHunger(food);
        
    }
}
