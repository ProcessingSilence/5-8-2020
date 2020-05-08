using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadow : MonoBehaviour
{
    public GameObject following;
    private Vector3 objectPos;
    public float shadowYOffset = 2;

    void Update()
    {
        if (following != null)
        {
            objectPos = new Vector3(following.transform.position.x, following.transform.position.y + shadowYOffset,
                following.transform.position.z);
            transform.position = objectPos;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}