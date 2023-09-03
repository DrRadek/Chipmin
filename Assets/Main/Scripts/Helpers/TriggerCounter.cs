using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCounter : MonoBehaviour
{
    [System.NonSerialized] public bool isEmpty = true;

    int count = 0;

    float timeSinceLastCollision = 0;

    public int Count { 
        get => count;
        set 
        {
            count = value;
            isEmpty = count == 0;
        } 
    }

    void OnTriggerEnter()
    {
        Count++;
    }

    private void OnTriggerStay(Collider _)
    {
        timeSinceLastCollision = 0;
    }

    void OnTriggerExit()
    {
        Count--;
    }
    private void OnDisable()
    {
        Count = 0;
    }

    private void FixedUpdate()
    {
        if (timeSinceLastCollision > 0.04f)
        {
            Count = 0;
        }

        timeSinceLastCollision += Time.deltaTime;
    }
}
