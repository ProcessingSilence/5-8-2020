using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public Transform Player;

    private Rigidbody rb;

    public float speed;

    private Vector3 ChargeLocation;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
        ChargeLocation = (Player.position - transform.position) ;
        StartCoroutine(DestroyMyself());
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.LookAt(Player);
        rb.AddForce((ChargeLocation).normalized * speed * Time.smoothDeltaTime);
    }

    IEnumerator DestroyMyself()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
