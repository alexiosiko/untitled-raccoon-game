using UnityEngine;

public class RaccoonController : MonoBehaviour
{
    private Animator animator;
    private Transform cameraTransform;

    // Variables to store smoothed input values
    private float smoothHorizontal = 0f;
    private float smoothVertical = 0f;

    // Smoothing speed
    public float inputSmoothingSpeed = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Capture raw input
        float rawHorizontal = Input.GetAxis("Horizontal");
        float rawVertical = Input.GetAxis("Vertical");

        // Smooth input transitions
        smoothHorizontal = Mathf.Lerp(smoothHorizontal, rawHorizontal, inputSmoothingSpeed * Time.deltaTime);
        smoothVertical = Mathf.Lerp(smoothVertical, rawVertical, inputSmoothingSpeed * Time.deltaTime);

        // Update Animator parameters
        animator.SetFloat("Horizontal", smoothHorizontal);
        animator.SetFloat("Vertical", smoothVertical);
    }
}
