using System.Collections;
using UnityEngine;

public class particleExplosion : MonoBehaviour
{
    private bool die = false;
    // Wait 3 seconds, then destroy myself.
    private void Update()
    {
        if (die = false)
        {
            die = true;
            StartCoroutine(waitBeforeDestroy());
        }
    }

    IEnumerator waitBeforeDestroy()
    {
        yield return new WaitForSeconds(8);
        Destroy(gameObject);
    }
}
