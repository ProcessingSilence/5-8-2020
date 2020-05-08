// Created by Liam W. Day
// Contact: lwday@quinnipiac.edu

using System.Collections;
using UnityEngine;
/*
 *                                               * * REMINDER * *
 * This script should be attached to an empty gameObject that acts as a parent to the used gameObject.
 *
 * This is to make it easier & optimized to disable everything in the used gameObject instead of disabling each component individually.
*/
public class KillWhenFallen : MonoBehaviour
{
    // Developer determines if object should delete itself or not after the child object is destroyed.
    public bool DoINotDeleteMyself;
    
    // Determine if enemy died before instantiating particle explosion.
    private bool haveIdiedUp; 
    
    // Determine if enemy died to disable everything and wait 5 seconds before deletion.
    private bool haveIdiedFix; 
    
    // Death explosion gameObject to spawn.
    public GameObject deathExplosion;

    // Set explosion as child to main parent.
    private GameObject explosionChild; // (This is done since the parent is getting destroyed anyway,
                                       // so no effort needs to be made to destroy the explosion gameObject).
    
    // Determines what Y-position is considered a falling death.
    public float fallDeathYPos = 35;
    
    // Child object to disable.
    public GameObject childToKill; // (This name would be bad out of context wouldn't it?)
    
    // Seconds before object gets deleted.
    public float secondsBeforeDelete = 1.5f; // (1.5 seconds recommended).

    // Child's transform.
    private Transform childPos;

    public bool isPlayer;

    public AudioSource my_Audio;

    public AudioClip playerDeath;

    public AudioClip enemyDeath;

    private void Start()
    {
        childPos = childToKill.GetComponent<Transform>();
        my_Audio = gameObject.GetComponent<AudioSource>();
    }

    // If transform position less than fallDeathYPos, enable particle explosion gameObject. 
    // This is done in FixedUpdate so the deathExplosion is guranteed to spawn before this gameObject gets deleted.
    void FixedUpdate()
    {
        if (childToKill != null)
        {
            if (childToKill.transform.position.y < fallDeathYPos && haveIdiedFix == false)
            {
                Debug.Log("I'm ded");
            
                // Set haveIdiedFix to true so if statement doesn't repeat.
                haveIdiedFix = true;
            
                // Instantiate explosion at child's position, set it to explosionChild slot...
                explosionChild = Instantiate (deathExplosion, new Vector3(childPos.position.x, childPos.position.y, childPos.position.z), childPos.rotation);
                // ...set it as child to main parent.
                explosionChild.transform.SetParent(gameObject.transform);
            }
        }
    }

    // If child's transform position less than fallDeathYPos, disable child, start deletion coroutine after chosen seconds.
    void Update()
    {
        if (childToKill != null)
        {
            if (childToKill.transform.position.y < fallDeathYPos && haveIdiedUp == false)
            {
                if (isPlayer == false)
                {
                    // Play enemy death audio.
                    my_Audio.clip = enemyDeath;
                    my_Audio.Play();
                    // Set haveIdiedUp to true so if statement doesn't repeat.
                    haveIdiedUp = true;
                    childToKill.SetActive(false);
                    StartCoroutine(waitBeforeDeletion());
                    

                }
                else if (isPlayer)
                {
                    childToKill.GetComponent<PlayerController>().currentHealth = -1000;
                    
                    // Play player death audio.
                    my_Audio.clip = playerDeath;
                    my_Audio.Play();
                }
                else if (childToKill != null)
                {
                    childToKill.GetComponent<PlayerController>().currentHealth = -1000;
                }
                else
                {
                    // Have the script disable itself when player is null.
                    gameObject.GetComponent<KillWhenFallen>().enabled = false;
                }
            }
        }



    }
    // Wait the chosen amount of seconds, delete gameObject if DoINotDeleteMyself == false, else delete explosionChild
    // for optimization purposes.
    private IEnumerator waitBeforeDeletion()
    {
        yield return new WaitForSeconds(secondsBeforeDelete);
        
        if (DoINotDeleteMyself == false)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(explosionChild);
        }
    }
}
