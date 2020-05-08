using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GOAL : MonoBehaviour
{
    // 0: Inactive.  1: Play win animation, tell pauseGame script to change to next scene.  2: Stop win if statement from repeating.
    private int haveIPlayed;

    // float variable that determines how fast the gameObject should spin. Positive value spins to left, negative to right.
    private float spinSpeed = -100f;
    // Access gameObject that pauses the game and determines if game should be restarted based on death or win.
    public GameObject pauseObject;
    // Access the pauseGame script from the gameObject that pauses the game to tell it that player has won the level.
    private pauseGame my_pauseGame_script;

    private Timer my_Timer_script;
    private GameObject TimerObj;

    // Start is called before the first frame update
    void Start()
    {
        // stop the particle system at the start, it only needs to run when player touches gameObject
        gameObject.GetComponent<ParticleSystem>().Stop();
        // get the pauseGame script from the pause gameObject.
        my_pauseGame_script = pauseObject.GetComponent<pauseGame>();
        TimerObj =  GameObject.FindGameObjectWithTag("Timer");
        my_Timer_script = TimerObj.GetComponent<Timer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (haveIPlayed == 1)
        {
            haveIPlayed = 2;
            spinSpeed = -800f;
            my_Timer_script.levelWon = 1;
        }
        
        // Rotate endlessly, multiply by spinSpeed to control the rotation speed.
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);    
    }

    // when gameObject enters 3d trigger
    private void OnTriggerEnter(Collider other)
    {
        // If the player is touching gameObject and the boolean is set to false (meaning it has not been touched by player
        // before).
        if (other.gameObject.CompareTag("Player") && haveIPlayed == 0)
        {
            // Set the haveIPlayed boolean to true first so that the if statement does not repeat
            haveIPlayed = 1;
            // play win particle system
            gameObject.GetComponent<ParticleSystem>().Play();
            // play win sound
            gameObject.GetComponent<AudioSource>().Play();            
            // set the enablePausing int in the pauseGame script to 3 so that the next stage can be accessed.
            my_pauseGame_script.enablePausing = 3;
            Destroy(other.gameObject);
        }
    }
}
