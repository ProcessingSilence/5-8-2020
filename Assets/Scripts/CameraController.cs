using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    public float turnSpeed = 4.0f;

    public GameObject playerObject;
    private Transform player;

    public Vector3 offset;

    private float originalYOffset;

    public float startingYOffset;
    // Gets y angle and changes it from mouse y while making sure it doesn't go over the y limit.
	// Passes it back to offset.y after its changing and checking is done.
    private float yPos;


    void Start()
    {
        originalYOffset = offset.y;
        
        // Set starting y position to given public float originalYOffset
        offset.y = startingYOffset;
        player = playerObject.GetComponent<Transform>();
        // Revolve around the player object, angle position is based on mouseX
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        Debug.Log(offset);

        // Set transform position to player position and offset at start.
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }


    private void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            // Get current offset.y into yPos.
            yPos = offset.y;
            // Subtract from yPos based on mouseY.
            yPos -= Input.GetAxis("Mouse Y");
            // Set yPos to max limit if over max.
            if (yPos > originalYOffset)
                yPos = originalYOffset;
            // Set yPos to min limit if under min.
            if (yPos < 0)
                yPos = 0;
            // Set offset.y to yPos var.
            offset.y = yPos;
        }
    }

    void LateUpdate()
    {
        if (Time.timeScale > 0 && player != null)
        {
            // Revolve around the player object, angle position is based on mouseX
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            //Debug.Log(offset);
            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }
        else if (player == null)
        {
            gameObject.GetComponent<CameraController>().enabled = false;
        }
    }
}