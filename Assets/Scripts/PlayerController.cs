using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 20.0f;
    public float jumpHeight;
    private Rigidbody rb;

    // [1]
    // Instantiate heatlh bar and set it to healthObj,
    // set its position to (133,0),
    // get its script and set to my_HealthBar_script,
    // set PlayerController script to my_PlayerController_script in Healthbar so values can be passed back and forth between scripts,
    // set it as child to the Canvas (find Canvas via "Canvas" tag),
    // send connect confirmation bool to HeathBar script
    // recieve connect confirmation bool from HealthBar script;
    private GameObject canvas;
    public GameObject healthObj;
    private HealthBar my_HealthBar_script;

    // Public bool that determines whether to show health bar or not.
    public bool spawnHealthObj = true;
    
    public int maxHealth = 3;
    public int currentHealth;

    // Amount of damage taken on damage touch
    public int damageAmt = 1;

    public bool connect;
    
    // See HoldingJump()
    private bool notHeldDown;
    
    Vector3 South;

    private float tempY;

    public float raycastDist = 3f;

    private int wonGame;
    private GameObject sceneManagerObject;
    private pauseGame my_pauseGame_script;

    public GameObject deathExplosion;

    public bool isDead;

    public float moveHorizontal;
    public float moveVertical;

    // Determines if player gets facked over by a tornado.
    // 0 = inactive, 1 = activate death IEnumerator, 2 = Force chaotic tornado movement upon player
    private int tornadoFacked;

    private RaycastHit hit;
    
    public Material DarkShadow;
    public Material LightShadow;

    public Projector shadowProjector;

    public GameObject rotatingObject;

    private AudioSource my_Audio;

    public AudioClip hurtSound1;
    public AudioClip hurtSound2;
    public AudioClip hurtSound3;
    public AudioClip jumpSound;
    public AudioClip landSound;

    private bool playLandingSound = true;

    public KillWhenFallen my_KillWhenFallen_script;
    
    void Awake()
    {
        if (spawnHealthObj)
        {
            // [1]{
            healthObj = Instantiate(healthObj);
            my_HealthBar_script = healthObj.GetComponent<HealthBar>();
            my_HealthBar_script.playerObj = gameObject;
            canvas = GameObject.FindGameObjectWithTag("Canvas");
            healthObj.transform.parent = canvas.transform;
            my_HealthBar_script.connect = true;
            if (connect)
            {
                Debug.Log("Healthbar connected to PlayerController");
            }
        }


        // }
        
        rb = GetComponent<Rigidbody>();
        South = new Vector3(0, -1, 0);
        sceneManagerObject = GameObject.FindGameObjectWithTag("SceneManager");
        my_pauseGame_script = sceneManagerObject.GetComponent<pauseGame>();
        currentHealth = maxHealth;
        my_Audio = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (spawnHealthObj)
        {
            healthObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,74, 0);
            healthObj.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.16f, 0.16f);
        }
    }

    void FixedUpdate()
    {
        // Set pauseGame's sceneNum to 2 (resets the scene), kill player with isDead bool.
        if (currentHealth <= 0)
        {
            my_pauseGame_script.sceneNum = 2;           
            Instantiate(deathExplosion, new Vector3 (transform.position.x,transform.position.y,transform.position.z), transform.rotation);
            if (spawnHealthObj)
            {
                my_HealthBar_script.slider.value = 0;
            }

            isDead = true;
        }
        /*
        // Destroy the ball if it falls off the map
        if (transform.position.y <= -20)
            currentHealth = 0;
         */      
    }
    
    void Update()
    {
        //  Start the coroutine and set tornadoFacked int to 2 which sends the player flying upwards while violently shaking.
        if (tornadoFacked == 1)
        {
            tornadoFacked = 2;
            StartCoroutine(TornadoDeath());
        }

        // Send player flying upwards while violently shaking in random directions.
        if (tornadoFacked == 2)
        {
            rb.AddForce(Random.Range(-200f, 200f),200,Random.Range(-200f, 200f) ) ;
        }

        // Get control input from player, it determines the movement of the player's xz velocity.
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Limit the velocity when the player is not being controlled in any direction.
        // If the player is touching object tagged "Ramp", the player will not slow down and remain slippery.
        if (moveHorizontal == 0 && moveVertical == 0 && IsGrounded() && !RampCheck())
        {
            rb.velocity = rbVelocity(rb.velocity, 10);
        }
        
        
        // Prevent ball from constantly sliding
        if (GetComponent<Rigidbody>().velocity.magnitude > maxSpeed)
        {
            rb.velocity = rbVelocity(rb.velocity, maxSpeed);
        }
        
        // The position of the camera determines where the force will move the ball in the x and z axis
        Vector3 relativeMovement = rotatingObject.transform.TransformVector(movement);
        relativeMovement.y = 0;
        
        rb.AddForce(relativeMovement * (maxSpeed * 5));

        // Set wonGame to 2 so if statement doesn't repeat
        if (wonGame == 1)
        {
            wonGame = 2;
            my_pauseGame_script.enablePausing = 3;
            Destroy(gameObject);
        }
        
        // Destroy player after recieving "isDead" bool
        if (isDead)
        {
            my_KillWhenFallen_script.my_Audio.Play();
            Destroy(gameObject);
        }
        
        // Check grounded raycast, and jump if ground is detected.
        // Also start coroutine to check if player is holding down space.
        if (Input.GetKeyDown("space") && IsGrounded())
        {
            notHeldDown = false;
            StartCoroutine(HoldingJump(relativeMovement));
            //StartCoroutine(moveDown());
            //Debug.Log("Space pressed");
            rb.AddForce(relativeMovement.x * (maxSpeed * 9), jumpHeight * 2, relativeMovement.x * (maxSpeed * 9));
            my_Audio.clip = jumpSound;
            my_Audio.Play();
        }

        // Add a slight amount of control force as the player falls.
        if (rb.velocity.y < 0)
        {
            rb.AddForce(relativeMovement.x * (maxSpeed * 5), 0, relativeMovement.z * (maxSpeed * 5));  
        }

        if (IsGrounded())
        {
            shadowProjector.material = LightShadow;
        }
        else
        {
            shadowProjector.material = DarkShadow;
        }
        //else if (Input.GetKeyDown("space") && !IsGrounded())
        //{
        //    Debug.Log("NOT Space pressed");
        //}
        //Debug.DrawLine(transform.position, South, Color.green);

        if (IsGrounded() && playLandingSound)
        {
            playLandingSound = false;
            my_Audio.clip = landSound;
            my_Audio.Play();
        }

        if (IsGrounded() == false && playLandingSound == false)
        {
            playLandingSound = true;
        }

    }

    // Get raycast position based on South position (-1 y)
    bool IsGrounded() 
    {
        return Physics.Raycast(transform.position, South, raycastDist);
    }
    
    
    // Get grounded raycast, and then send an endless raycast downward detecting tag of object below.
    // If object is tagged as "Ramp", return true.
    bool RampCheck() 
    {
        if (IsGrounded())
        {
            if (Physics.Raycast(transform.position, South, out hit) && hit.transform.tag == "Ramp")
            {
                //Debug.Log("RAMP DETECTED.");
                return true;
            }
            return false;
        }
        return false;
    }

    
    // Checks if the player is holding down a jump button through a for loop that iterates every 0.01 seconds.
    // If space is held down and the y velocity is more than -0.1, add slight amount of force upwards so player jump is longer.
    // If the for loop detects the space isn't held or the y velocity less than -0.1 then it cuts the for loop off.
    IEnumerator HoldingJump(Vector3 relativeMovement)
    {
        if (notHeldDown == false)
        {
            notHeldDown = true;
            for (int i = 0; i < 7; i++)
            {
                // Do not allow the player to jump higher when falling, check if y velocity less than -0.1
                if (Input.GetKey(KeyCode.Space) && rb.velocity.y  > -.01f)
                {
                    //Debug.Log(relativeMovement);
                    rb.AddForce((relativeMovement.x * 1.1f) * (maxSpeed * 10), jumpHeight * 0.1f, (relativeMovement.z* 1.1f) * (maxSpeed * 10));
                    Debug.Log("Space held.");
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    Debug.Log("Space NOT held.");
                    i = 11;
                }
            }
        }
    }

    void OnCollisionEnter(Collision _collision)
    {
        //player takes damage here
        if (_collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Touched damager");
            // Choose and play random hurt sound effect from 1 to 3.
            var damageSound = Random.Range(1,4);
            switch (damageSound)
            {
                case 1:
                {
                    my_Audio.clip = hurtSound1;
                    break;
                }
                case 2:
                {
                    my_Audio.clip = hurtSound2;
                    break;
                }
                case 3:
                {
                    my_Audio.clip = hurtSound3;
                    break;
                }
            }
            my_Audio.Play();
            
            currentHealth -= damageAmt;
            // Send bool signal to myHealthBar script to subtract health.
            my_HealthBar_script.takeDamage = true;
        }
        
        // Start tornadoFacked coroutine procedure after player touches tornado hitbox.
        if (_collision.gameObject.tag == "Tornado")
        {
            tornadoFacked = 1;
        }
        
        // Kill the player instantly when colliding with object tagged "InstaKill"
        if (_collision.gameObject.tag == "InstaKill")
        {
            //my_KillWhenFallen_script.my_Audio.Play();
            currentHealth = 0;
        }
    }

    // Wait 4 seconds before killing off the player.
    IEnumerator TornadoDeath()
    {
        yield return new WaitForSeconds(4);
        currentHealth = 0;
    }

    // Limit speed on x and z positions        
    // Get the y velocity in a temp value, limit all the velocities, and then place the temp y velocity back into the rb y velocity slot.
    Vector3 rbVelocity(Vector3 rb, float speedLimit)
    {
        var tempVel = rb.y;
            
        rb.y = 0.0f;
        rb = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, speedLimit);
        rb.y = tempVel;
        return rb;
        //GetComponent<Rigidbody>().velocity = rb;
    }


    /*
    IEnumerator moveDown()
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < 10; i++)
        {
            if (!IsGrounded())
            {
                yield return new WaitForSeconds(0.01f);
                rb.AddForce(0, -i*20, 0);
            }
        }
    }
    */
}