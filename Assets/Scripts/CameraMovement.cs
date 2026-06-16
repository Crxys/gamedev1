using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The target the camera will follow
    public float smoothSpeed = 5f; // The speed of the camera's
    public Vector3 offset; // The offset from the target's position
    private Vector3 velocity = Vector3.zero; // Used for SmoothDamp
    void LateUpdate()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position + offset;
        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        // Update the camera's position
        transform.position = smoothedPosition;

        // Optionally, you can also make the camera look at the target
        transform.LookAt(target);
    }
}
