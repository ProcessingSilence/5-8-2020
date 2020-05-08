using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    // This is a Singleton of the particle Spawner. there is only one instance 
    // of this Spawner, so we can store it in a static variable named s.
    static public Spawner theSpawner;
    static public List<ParticleUnity> ParticleUnitys;
    
    // These fields allow you to adjust the spawning behavior of the spheres
    [Header("Set in Inspector: Spawning")]
    public GameObject particleUnityPrefab;
    public Transform particleAnchor;

    // Spawn-rate delay, can be changed in editor.
    public float spawnDelay = 0.01f;

    // Number to count the amount of particles spawned.
    private int countParticles = 1;

    // Spawning radius of where the particles can randomly spawn.
    public float radius = 5f;
    void Awake()
    {
        //Set the Singleton S to be this instance of particle spawner
        theSpawner = this;
        //Start instantiation of the particles
        ParticleUnitys = new List<ParticleUnity>();//the list holding the particles
        InstantiateParticleUnitys();
        //myText.text = countParticles + "/" + numParticles;
    }

    //a method to spawn the particles
    public void InstantiateParticleUnitys()
    {
        GameObject go = Instantiate(particleUnityPrefab, Random.insideUnitSphere * radius + transform.position,
            Random.rotation);
        ParticleUnity b = go.GetComponent<ParticleUnity>();
        b.transform.SetParent(particleAnchor);
        ParticleUnitys.Add(b);
        Invoke("InstantiateParticleUnitys", spawnDelay);
        countParticles++;
    }
}

