using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class titleScreenScript : MonoBehaviour
{
    public Text FlashingTitle;

    public Rigidbody playerBall;

    // On = true, Off = false;
    private bool OnOff;

    // Determines if game has started or not.
    private bool GameStarted;
    
    // Dark image slot that fades in when the player dies or wins. It fades out when a scene is loaded. 
    public CanvasGroup fadeInImage;

    private AudioSource my_Audio;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn(-0.1f));
        StartCoroutine(FlashingTitleScreen());
        my_Audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && GameStarted == false)
        {
            GameStarted = true;
            OnOff = false;
            print("enter key was pressed");
            StartCoroutine(PlayerAnimation());
        }

        if (OnOff)
        {
            FlashingTitle.enabled = true;
        }
        else
        {
            FlashingTitle.enabled = false;
        }
    }

    IEnumerator FlashingTitleScreen()
    {
        if (GameStarted != true)
        {
            yield return new WaitForSeconds(0.5f);
            OnOff = !OnOff;
            StartCoroutine(FlashingTitleScreen());
        }
        else
        {
            OnOff = false;
        }
    }

    IEnumerator PlayerAnimation()
    {
        // Player jumps from the start and moves
        my_Audio.Play();
        playerBall.AddForce(0,800,0);
        // Player moves
        for (int i = 0; i < 6; i++)
        {
            playerBall.AddForce(0,0,-200);
        }
        // ...Then jumps again at the platform
        yield return new WaitForSeconds(1.2f);   
        playerBall.AddForce(0,1200,0);
        my_Audio.Play();
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(FadeIn(0.1f));
        yield return new WaitForSeconds(0.11f);           
    }

    IEnumerator FadeIn(float alpha)
    {
        for (int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(0.01f);
            fadeInImage.alpha += alpha;
        }
        if (alpha > 0)
        {
            SceneManager.LoadScene("Level 1"); 
        }
    }
}
