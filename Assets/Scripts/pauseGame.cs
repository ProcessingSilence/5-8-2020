using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseGame : MonoBehaviour
{
    // Slot for text that tells player that the game is paused.
    
    public Text pauseText;
    
    // Dark image slot that fades in when the player dies or wins. It fades out when a scene is loaded. 
    public CanvasGroup fadeInImage;
    
    // Grey image slot that covers the whole screen when paused, this is to prevent the player from exploiting the ability
    // to stop everything by pausing the game.
    public GameObject pauseImage;
    
    // Boolean that determines if game is paused or not
    private bool isPaused;
    

    // Integer that determines different fade options and scene reloading
    /* 0: Fade out after scene starts by 0.1 second increments.
     * 1: If the game has already started & playerObject is null, use coroutine to search for player before determining player is dead.
     * 2: Fade in after player dies by 0.1 second increments. Once faded in all the way, restart the current scene.
     * 3: Fade in all the way after the player wins by 0.1 second increments. Once fades all the way, load the next scene.
     */
    public int enablePausing = 0;

    // player gameObject slot
    private GameObject playerObject;

    // Meant to be used at titleScreen, applies player wins effect to move to next scene.
    private bool titleScreen;
    
    // Scene integer number.
    public int sceneNum = 0;
    
    // String slot for scene name.
    public string currentSceneName;
    
    // String number of scene extracted from sceneName string.
    public string numChar;

    private Timer my_Timer_script;

    private GameObject timerObj;
    
    public GameObject Camera1;
    public GameObject Camera2;
    void Start()
    {
        var currentScene = SceneManager.GetActiveScene();
        // Create variable currentSceneName to get the scene name from active scene variable
        var currentSceneName = currentScene.name;

        if (currentSceneName == "title")
        {
            titleScreen = true;
        }

        // Disable pauseText text
        pauseImage.SetActive(false);
        timerObj =  GameObject.FindGameObjectWithTag("Timer");
        my_Timer_script = timerObj.GetComponent<Timer>();
        // Set the alpha of the fadeInImage to 1 to make it opaque.
        fadeInImage.alpha = 1;

    }
    
    IEnumerator FadeIn()
    {
        // enablePausing has 4 different uses depending on which number it is given
        
        // 0: Fade out after scene starts by 0.1 second increments.
        if (enablePausing == 0)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < 11; i++)
            {
                yield return new WaitForSeconds(0.01f);
                fadeInImage.alpha -= .1f;
            }
        }
        
        // 1: Only used to stop 0's if statement from repeating.

        // 2: Fade in after player dies by 0.1 second increments. Once faded in all the way, restart the current scene.
        if (enablePausing == 2)
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 11; i++)
            {
                yield return new WaitForSeconds(0.01f);
                fadeInImage.alpha += .1f;
            }
            // Add to death counter in Timer script;
            my_Timer_script.deaths += 1;
            Debug.Log("Deaths:" + my_Timer_script.deaths);
            // Create variable currentScene to get the active scene
            var currentScene = SceneManager.GetActiveScene();
            // Create variable currentSceneName to get the scene name from active scene variable
            var currentSceneName = currentScene.name;
            // Load the next scene based on the currentSceneName string
            SceneManager.LoadScene(currentSceneName);
        }

        // 3: Fade in all the way after the player wins by 0.1 second increments. Once fades all the way, load the next scene
        if (enablePausing == 3)
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 11; i++)
            {
                yield return new WaitForSeconds(0.01f);
                fadeInImage.alpha += .1f;
            }
            
            // Get the active scene, then convert it to a string.
            var getcurrentSceneName = SceneManager.GetActiveScene();
            currentSceneName = getcurrentSceneName.name;
            
            // When the final level is complete, load the ending scene.
            if (currentSceneName == "Level 9")
            {
                SceneManager.LoadScene("ending");
            }
            
            // Remove number from scene name string, replace it with new scene number.
            if (currentSceneName != "title" || currentSceneName != "Level 9")
            {        
                // Get the one-digit number from the scene name !!(DOES NOT WORK WITH DOUBLE-DIGIT NUMBERS)!!
                numChar = currentSceneName.Substring(6, 1);
        
                // Convert sceneNum string to integer.
                sceneNum = int.Parse(numChar);
                sceneNum++;
                            
                // Remove number from scene name string, replace it with new scene number.
                currentSceneName = currentSceneName.Remove(6, 1);
                currentSceneName = currentSceneName.Insert(6, sceneNum.ToString());
            
                // Change scene based on currentSceneName string.
                SceneManager.LoadScene(currentSceneName);
            }
            // If none of these if statements apply, either the scene is the Title or the wrong scene is put in.
            else if (currentSceneName == "title" || currentSceneName == "ending")
            {
                SceneManager.LoadScene("Level 1");
            }
            // If all fails, load Level 1.
            else
            {
                SceneManager.LoadScene("Level 1");
                Debug.Log("Scene failed to load correctly.");
            }

        }
        // Return null if none of the if statements activate.
        yield return null;
    }



    void Update()
    {
        if (titleScreen)
        {
            if (Input.anyKey && !Input.GetKeyDown(KeyCode.Escape))
            {
                enablePausing = 3;SceneManager.LoadScene(currentSceneName);
            }
        }

        // Start finding the player as the game begins
        // Only fade out if the playerObject slot is null.
        if (playerObject == null && enablePausing == 0)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(FadeIn());
            // Set enablePausing to 1 so that this if statement does not repeat.
            enablePausing = 1;
        }

        // If the game has already started and the playerObject is null, use a coroutine to search for the player before
        // determining that the player is dead.
        if (playerObject == null && enablePausing == 1)
        {
            StartCoroutine(waitBeforeDeterminingDead());           
        }

        // Player is confirmed to be dead; fade in and restart the scene.
        if (enablePausing == 2)
        {
            StartCoroutine(FadeIn());
        }

        // Player has won the level; fade in and load next scene.
        if (enablePausing == 3)
        {
            StartCoroutine(FadeIn());
        }
        
        // If ESC is pressed, isPaused is equal to its opposite.
        if (Input.GetKeyDown(KeyCode.Escape) && enablePausing != 2 )
            isPaused = !isPaused;

        // If the boolean is false, and the player isn't dead or has won, time moves as normal.
        if (isPaused == false && (enablePausing != 2 || enablePausing != 3))
        {
            pauseImage.SetActive(false);
            Camera1.SetActive(true);
            Camera2.SetActive(true);
            Time.timeScale = 1;         
        }
        
        //  ^
        //  |  |    <<< These two conditions switch each time ESC is pressed.
        //     v
        
        // If the boolean is true, and the player isn't dead or has won, time stops.
        else if (isPaused && (enablePausing != 2 || enablePausing != 3))
        {
            Time.timeScale = 0;
            pauseImage.SetActive(true);
            Camera1.SetActive(false);
            Camera2.SetActive(false);
        }
    }

    // Wait 0.1 seconds to see if checkpoint will spawn player before confirming that the player is dead.
    IEnumerator waitBeforeDeterminingDead()
    {
        // After waiting 0.1 seconds, if the player isn't found, enablePausing = 2 to confirm player death.
        yield return new WaitForSeconds(0.1f);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObject == null)
            enablePausing = 2;
    }


}
