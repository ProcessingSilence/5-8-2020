using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemyHitBox : MonoBehaviour
{
    public GameObject[] enemyList;

    // 0: inactive, 1: Activate from trigger, 2: Activate enemy AI
    private int touched;
    // Start is called before the first frame update

    void Start()
    {
        for (var i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].GetComponent<rotateTowardsEachOther>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (touched == 1)
        {
            touched = 2;
            // Go through the enemy array and activate the enemies' scripts.
            for (var i = 0; i < enemyList.Length; i++)
            {
                enemyList[i].GetComponent<rotateTowardsEachOther>().enabled = true;
            }
            
            // Once done cycling through array the GameObject destroys itself; it has no other use left.
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && touched == 0) 
        {
            touched = 1;
        }
    }
}
