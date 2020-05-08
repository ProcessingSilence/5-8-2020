/*
 * The way that the player controls work is that a force is added to the player based on an object's position + the
 * direction input by the player.
 *
 * The camera was originally the object that detrermines the force direction, but it causes problems when it begins to
 * hover above the player.
 *
 * This object was created to determine the force direction based on the x and z axis of the camera, while constantly
 * following the player's y position.
 */
using UnityEngine;

public class ForceDirectionToPlayer : MonoBehaviour
{
    public Transform cameraTrans;
    public Transform playerTrans;
    
    void FixedUpdate()
    {
        transform.position = new Vector3(cameraTrans.position.x, playerTrans.position.y, cameraTrans.position.z);
    }

    private void Update()
    {
        transform.LookAt(playerTrans.position);
    }
}
