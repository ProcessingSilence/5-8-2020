using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class tornadoMove : MonoBehaviour
{
    public Vector3 translateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(translateSpeed.x,translateSpeed.y,translateSpeed.z * Time.deltaTime) );
    }
}
