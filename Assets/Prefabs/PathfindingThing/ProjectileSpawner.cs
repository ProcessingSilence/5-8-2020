/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    private bool isRunning;

    public float SpawnRate;

    public GameObject Projectile;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnProjectile());
    }

    void Update()
    {
        if (isRunning == false)
        {
            isRunning = true;
            StartCoroutine(SpawnProjectile());
        }
    }

    IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(SpawnRate);
        Instantiate(Projectile, transform.position, transform.rotation);
        isRunning = false;
    }
}
*/