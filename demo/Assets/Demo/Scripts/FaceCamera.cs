using UnityEngine;

/// <summary>
/// Face Camera
/// Description: Have an object always face the camera.
/// </summary>

public class FaceCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // face the camera
        transform.LookAt(Camera.main.transform);
    }
}