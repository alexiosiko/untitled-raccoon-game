using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 1.0f;
    public float bobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 offset;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust rotation
        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Scroll input for zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float distance = offset.magnitude;
        distance -= scrollInput * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
        offset = offset.normalized * distance;

        // Update camera position
        transform.position = target.position + rotation * offset; // Vector3.up to give a little oomf up
        transform.LookAt(target.position + Vector3.up /2 );
    }
}
