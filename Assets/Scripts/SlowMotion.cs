using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    public int slowTime;
    public float changeTime = 1;
    public float timeSoFar;
    void Update()
    {
        Time.timeScale = changeTime;
        timeSoFar = Time.timeScale;
        if (slowTime == 1)
        {
            slowTime = 2;
            StartCoroutine(TimeChange(-0.1f));
        }
        
        if (slowTime == 3)
        {
            slowTime = 4;
            StartCoroutine(TimeChange(0.1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            slowTime = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            slowTime = 3;
        }
    }

    IEnumerator TimeChange(float timeIncrement)
    {
        
        if (slowTime == 2)
            changeTime = 1f;
        if (slowTime == 4)        
            changeTime = .5f;
        

        for (var i = 0; i < 9; i++)
        {
            yield return new WaitForSeconds(0.01f);
            changeTime += timeIncrement;
        }

        yield return new WaitForSeconds(0.1f);
    }
}
