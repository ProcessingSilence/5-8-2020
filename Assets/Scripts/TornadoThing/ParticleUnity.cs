using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleUnity : MonoBehaviour
{
    public Vector3 initialVel;
    public Vector3 newPos;
    public Rigidbody rb;
    private Vector3 fvec;
    Vector3 updateForce;
    

    // Start is called before the first frame update
    void Awake()
    {
        //set the initial position of a spawned particle
        transform.position = Random.insideUnitSphere * 2.0f + Vector3.up * 2;
        //set initial velocity of the spawned particle
        initialVel = 0.0f * Vector3.one;
        rb = GetComponent<Rigidbody>();
        rb.velocity = initialVel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Create a force field
        newPos = transform.position; //get the particle postion
        updateForce = 4.0f * Vector3.Cross(Vector3.up, newPos);
        
        fvec = transform.position - Vector3.up * transform.position.y;
        updateForce += -10.0f * fvec;
        updateForce += Vector3.up;
        // Push the particle within the force field
        rb.AddForce(updateForce);
        
        // If the particle goes outside a 1200x1200 area, destroy the particle.
        if (transform.position.x > 600 || transform.position.x < -600
            || transform.position.z > 600 || transform.position.z < -600)
        {
            Destroy(gameObject);
        }
    }
}
