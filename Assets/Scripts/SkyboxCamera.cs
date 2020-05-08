using UnityEngine;

public class SkyboxCamera : MonoBehaviour
{
    public Transform playerCamera;

    private void Update()
    {
        transform.rotation = playerCamera.rotation;
    }
}