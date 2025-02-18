using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;        // Reference to the player's transform
    public Vector3 offset;          // Offset position of the camera relative to the player
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    public Vector3 rotationAxis = Vector3.up; // Axis around which the camera will rotate
    public float rotationSpeed = 1.0f; // Speed of the subtle rotation in degrees per second

    void LateUpdate()
    {
        // Desired position of the camera
        Vector3 desiredPosition = target.position + offset;
        // Smoothly interpolate between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Update the camera's position
        transform.position = smoothedPosition;

        // Optionally, make the camera look at the player
        transform.LookAt(target);

        // Apply a subtle rotation around the specified axis
        transform.RotateAround(target.position, rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
