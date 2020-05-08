using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    private NavMeshAgent Mob;

    private Transform Player;

    public float ProjectileFireRange = 20.0f;

    public GameObject Projectile;

    public Transform ProjectileSpawner;

    public float SpawnRate;

    // 0: Not running, 1: Activate spawner, 2: Currently running
    private int isRunning;

    private AudioSource my_Audio;
    // Use this for initialization
    void Start()
    {
        Mob = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        my_Audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        Vector3 dirToPlayer = transform.position - Player.position;

        Vector3 newPos = transform.position - dirToPlayer;

        Mob.SetDestination(newPos);

        //
        if (distance < ProjectileFireRange)
        {
            if (isRunning != 2)
            {
                isRunning = 1;
            }
        }
        else
        {
            isRunning = 0;
        }

        switch (isRunning)
        {
            case 0:
            {
                break;
            }
            case 1:
            {
                isRunning = 2;
                StartCoroutine(SpawnProjectile());
                break;               
            }
        }

        IEnumerator SpawnProjectile()
        {
            my_Audio.Play();
            Instantiate(Projectile, ProjectileSpawner.position, ProjectileSpawner.rotation);           
            yield return new WaitForSeconds(SpawnRate);
            if (isRunning != 0)
            {
                isRunning = 1;                
            }
        }
    }
}
