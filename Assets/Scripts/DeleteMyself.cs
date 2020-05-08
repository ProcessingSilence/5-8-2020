using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMyself : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);
        }
    }
}
