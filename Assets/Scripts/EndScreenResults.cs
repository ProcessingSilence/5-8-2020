using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenResults : MonoBehaviour
{
    private float totalTime;
    private float totalDeaths;
    
    public Text totalTimeText;
    public Text totalDeathsText;

    public GameObject TimerObj;
    
    private Timer my_Timer_script;
    // Start is called before the first frame update
    void Start()
    {

        // Search for gameObjects with the timer tag before attempting to gather variables from script.
        if ( GameObject.FindGameObjectsWithTag("Timer").Length > 0)
        {
            // Get total time and deaths from Timer script and place them into the display text on endscreen.
            my_Timer_script = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
            totalTime = my_Timer_script.totalTime;
            totalDeaths = my_Timer_script.totalDeaths;
            totalTimeText.text = "With a time of: " + totalTime;
            // Make sure punctuation is correct when 1 death (not "deaths") occurs.
            if (totalDeaths == 1)
            {
                totalDeathsText.text = "...and " + totalDeaths + " death.";
            }
            else
            {
                totalDeathsText.text = "...and " + totalDeaths + " deaths.";
            }


            // Delete timer after everything is received, it no longer needs to count anything.
            Destroy(TimerObj);
        }
        // Else display "error" message
        else
        {
            totalTimeText.text = "With a time of -0";
            totalDeathsText.text = "...and infinity deaths";
        }     
    }
}
