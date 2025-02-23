using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class RaccoonController : MonoBehaviour
{
    public bool isGrounded;
    // Variables to store smoothed input values
    private float smoothHorizontal = 0f;
    private float smoothVertical = 0f;
    // Smoothing speed
    public float inputSmoothingSpeed = 5f;
    // Ground check settings
    float groundCheckOffset = 0.05f;
    float groundCheckDistance = 0.1f;
    void Update()
    {
		UpdateGrounded();
        // Capture raw input
        float rawHorizontal = Input.GetAxis("Horizontal");
        float rawVertical = Input.GetAxis("Vertical");

        // Smooth input transitions
        smoothHorizontal = Mathf.Lerp(smoothHorizontal, rawHorizontal, inputSmoothingSpeed * Time.deltaTime);
        smoothVertical = Mathf.Lerp(smoothVertical, rawVertical, inputSmoothingSpeed * Time.deltaTime);

        // Update Animator parameters
        animator.SetFloat("Horizontal", smoothHorizontal);
        animator.SetFloat("Vertical", smoothVertical);


        // Update Animator with ground status
        animator.SetBool("isGrounded", isGrounded);
		if (isGrounded)
			animator.applyRootMotion = true;

			

		// Turn off gravity if climbing
		if (animator.GetBool("Climbing"))
			rb.isKinematic = true;
		else
			rb.isKinematic = false;

        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
			if (animator.GetBool("Climbing"))
			{
				animator.SetTrigger("Jump");
				animator.SetBool("Climbing", false);
			}
			else
				animator.SetBool("Climbing", true);

        }


    }
	void UpdateGrounded() => isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.2f);
	void Awake()
    {
        animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
    }
	Rigidbody rb;
    Animator animator;
}
