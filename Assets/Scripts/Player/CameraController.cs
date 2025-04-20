using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public bool invertY = true;
    
    [Header("Target Settings")]
    public Transform target;
    public Vector3 targetOffset = Vector3.up * 0.5f;
    public float smoothSpeed = 0.125f;
    
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 1.0f;
    public float minVerticalAngle = -60f;
    public float maxVerticalAngle = 80f;
    
    [Header("Bobbing Settings")]
    public float bobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public bool enableBobbing = true;
    
    [Header("Zoom Settings")]
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 offset;
    private Vector3 smoothedPosition;
    private float defaultZoom;
    private float currentZoom;
    private float bobbingTimer = 0f;
    public static bool freeze = false;
    
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        offset = transform.position - target.position;
        defaultZoom = offset.magnitude;
        currentZoom = defaultZoom;
    }

    void LateUpdate()
    {
        if (freeze) return;
        if (target == null) return;

        HandleMouseInput();
        HandleZoom();
        UpdateCameraPosition();
    }

    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * (invertY ? -1 : 1);

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scrollInput * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    private void UpdateCameraPosition()
    {
        // Calculate desired position
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Vector3 desiredPosition = target.position + rotation * (offset.normalized * currentZoom);
        
        // Apply head bobbing if enabled and moving
        if (enableBobbing && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;
            float bobbingOffset = Mathf.Sin(bobbingTimer) * bobbingAmount;
            desiredPosition += transform.up * bobbingOffset;
        }
        else
        {
            bobbingTimer = 0f;
        }
        
        // Smooth movement
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        
        // Look at target with offset
        transform.LookAt(target.position + targetOffset);
    }
}