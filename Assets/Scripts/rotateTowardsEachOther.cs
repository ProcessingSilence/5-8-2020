using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

//CREDIT: https://www.youtube.com/watch?v=mKLp-2iseDc


public class rotateTowardsEachOther : MonoBehaviour
{
    // Target gameObject
    public GameObject target;
    private Transform targetTransform;

    public Light BottomLight;
    private float originalLightIntensity;
    
    // how fast the object will turn towards target transform
    public float turnSpeed = 6f;
    private float slowTurnSpeed;
    private float originalTurnSpeed;
    private float fastTurnSpeed;
    
    // Distance of when to stop turning.
    public float turnAroundDist;

    // Distance of when to charge at player;
    public float chargeDist;
    
    public float relativeSpeed = 150f; //movement speed of enemy
    private float slowRelativeSpeed;
    private float originalSlowRelativeSpeed;
    private float originalRelativeSpeed;
    
    public float translateSpeed = 20f;
    public float originalTranslateSpeed;
    public float slowTranslateSpeed;
    private bool haveIdied = false; // determine if enemy died or not so it does not die rapidly

    // Changes turnAroundDist during charging attack
    private float originalPlayerDistance;
    
    private bool IEnumeratorPlaying;

    private Color originalLightColor;
    
    private Rigidbody rb;

    // Player position + offset
    private Vector3 playerVector;
    
    // Make offset of player position so that the enemy won't constantly circle around the player from up close.
    public float playerOffset;

    private AudioSource my_Audio;
    public AudioClip anticipationSound;
    public AudioClip chargeSound;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        targetTransform = target.transform;
        slowTurnSpeed = turnSpeed / 5;
        originalTurnSpeed = turnSpeed;
        rb = gameObject.GetComponent<Rigidbody>();
        slowRelativeSpeed = relativeSpeed/5;
        originalSlowRelativeSpeed = slowRelativeSpeed;
        originalRelativeSpeed = relativeSpeed;
        originalTranslateSpeed = translateSpeed;
        slowTranslateSpeed = translateSpeed / 5;
        originalPlayerDistance = turnAroundDist;
        originalLightIntensity = BottomLight.intensity;
        originalLightColor = BottomLight.color;
        my_Audio = gameObject.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        playerVector = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);  
    }
    
    private void Update()
    {
        // prevent enemy from going into the z axis
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
        
        
        
        // only move when health is not equal to 0
        if (haveIdied == false)
        {
            transform.Translate(new Vector3(0,0,translateSpeed * Time.deltaTime) );
            rb.AddRelativeForce(new Vector3(0,0,relativeSpeed * Time.deltaTime) );
        }
        
        // Get position between our position and target position.
        Vector3 direction = targetTransform.position - transform.position;
        // Calculate the angle
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // Create a rotation from the angle
        //Debug.Log(angle);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        // Only rotate when player is 2 pts away from gameObject in any direction
        if (Vector3.Distance(transform.position, playerVector) > turnAroundDist)
        {          
            turnSpeed = originalTurnSpeed;
            relativeSpeed = originalRelativeSpeed;
        }
        else
        {
            if (IEnumeratorPlaying == false)
            {
                IEnumeratorPlaying = true;
                StartCoroutine(waitThenCharge());
            }
            turnSpeed = slowTurnSpeed;
            relativeSpeed = slowRelativeSpeed;
        }
    }

    private IEnumerator waitThenCharge()
    {
        yield return new WaitForSeconds(1.3f);

        if (Vector3.Distance(transform.position, playerVector) < chargeDist)
        {
            my_Audio.clip = anticipationSound;
            // Turn down volume since anticipation sound is louder than charging sound.
            my_Audio.volume = 0.8f;
            my_Audio.Play();
            // Turn off speed, make enemy guarenteed to face towards player.
            slowRelativeSpeed = 0;
            turnAroundDist = 50000;
            slowTurnSpeed = originalTurnSpeed * 3;
            slowRelativeSpeed = 0;
            // Enemy slowly moves back in anticipation
            translateSpeed = -0.1f;
            // enemy glows red as a warning
            BottomLight.color = new Color(.4f, 0, 0, 1);
            for (int x = 0; x < 20; x++)
            {
                BottomLight.intensity += 5;
                yield return new WaitForSeconds(0.05f);
            }
            my_Audio.clip = chargeSound;
            my_Audio.volume = 1f;
            my_Audio.Play();
            // Enemy charges at player with bright light
            BottomLight.color = originalLightColor;
            //color #003735

            BottomLight.intensity = originalLightIntensity;
            //slowTurnSpeed = originalTurnSpeed * 25f;
            translateSpeed = originalTranslateSpeed;
            slowRelativeSpeed = originalRelativeSpeed * 10;
            yield return new WaitForSeconds(0.03f);
            //slowTurnSpeed = 0;
            yield return new WaitForSeconds(0.22f);  
            // Enemy turns around after realising it missed (it won't look like this if it actually hits the player).
            slowTurnSpeed = originalTurnSpeed * 2;
            slowRelativeSpeed = originalRelativeSpeed * 10;
            yield return new WaitForSeconds(0.3f);
            // Return all stats back to normal.
            slowTurnSpeed = originalTurnSpeed / 5;
            slowRelativeSpeed = originalRelativeSpeed / 5;
            turnAroundDist = originalPlayerDistance;
        }
        IEnumeratorPlaying = false;
    }
}
