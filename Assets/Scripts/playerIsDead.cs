using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerIsDead : MonoBehaviour
{
    public GameObject Player;

    // Death = false, Win = true.
    public bool deathOrWin;

    private bool hasCoroutineStarted;
    
    private pauseGame my_pauseGame_script;
    public GameObject sceneManager;

    void Start()
    {
        my_pauseGame_script = sceneManager.GetComponent<pauseGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            if (!Player.active && hasCoroutineStarted == false)
            {
                hasCoroutineStarted = true;
                StartCoroutine(DetermineWinOrDeath());
            }
        }
    }

    IEnumerator DetermineWinOrDeath()
    {
        
        if (deathOrWin == false)
        {
            yield return new WaitForSeconds(1f);
            my_pauseGame_script.enablePausing = 2;
            gameObject.GetComponent<playerIsDead>().enabled = false;
        }
        else
        {
            yield return new WaitForSeconds(5f);
            my_pauseGame_script.enablePausing = 3;
            gameObject.GetComponent<playerIsDead>().enabled = false;
        }
    }
}
