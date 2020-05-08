using UnityEngine;

public class LockMouse : MonoBehaviour
{
    public GameObject pauseImage;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Update()
    {
        if (pauseImage == null)
        {
            pauseImage = GameObject.FindGameObjectWithTag("UI_Group");
        }

        //Escape input unlocks the Cursor
        if (Input.GetKey(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        // Make absolute sure that the mouse doesn't lock by unlocking the mouse while the pauseImage background is active.
        if (pauseImage.active)
            Cursor.lockState = CursorLockMode.None;

        // Clicking locks the mouse again
        if (Input.GetMouseButtonDown(0) && !pauseImage.active)
            Cursor.lockState = CursorLockMode.Locked;
    }
}
