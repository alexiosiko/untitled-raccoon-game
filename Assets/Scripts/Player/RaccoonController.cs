using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class RaccoonController : MonoBehaviour
{
	[SerializeField] Transform rigTransform;
    private float smoothHorizontal = 0f;
    private float smoothVertical = 0f;
    public float inputSmoothingSpeed = 5f;
    public float groundCheckDistance = 0.1f;
    public float jumpForce = 5f;
    private Animator animator;
    private Rigidbody rb;
	private Collider walkingCollider;
	float climbingHorizontalDistance = 1f;
	Vector3 centerOfRaccoon;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        walkingCollider = GetComponent<Collider>();
    }
    void Update()
    {
        UpdateAnimatorParameters();
        UpdateGroundedStatus();
		centerOfRaccoon = transform.position + Vector3.up / 4;
        UpdateInput();
        HandleClimb();
	}


	void UpdateGroundedStatus()
    {
        Vector3 boxCenter = walkingCollider.bounds.center;
        Vector3 halfExtents = walkingCollider.bounds.extents;
        bool isGrounded = Physics.BoxCast(boxCenter, halfExtents, Vector3.down, out RaycastHit hitInfo, Quaternion.identity, groundCheckDistance);
        animator.SetBool("isGrounded", isGrounded);

        Debug.DrawRay(boxCenter, Vector3.down * (hitInfo.distance > 0 ? hitInfo.distance : groundCheckDistance), isGrounded ? Color.green : Color.red);
    }

    void UpdateInput()
    {
        float rawHorizontal = Input.GetAxis("Horizontal");
        float rawVertical = Input.GetAxis("Vertical");
        smoothHorizontal = Mathf.Lerp(smoothHorizontal, rawHorizontal, inputSmoothingSpeed * Time.deltaTime);
        smoothVertical = Mathf.Lerp(smoothVertical, rawVertical, inputSmoothingSpeed * Time.deltaTime);
    }

    void UpdateAnimatorParameters()
    {
        animator.SetFloat("Horizontal", smoothHorizontal);
        animator.SetFloat("Vertical", smoothVertical);
    }

	void HandleClimb()
	{
		Debug.DrawLine(centerOfRaccoon, centerOfRaccoon + transform.forward * climbingHorizontalDistance, Color.red);
	    if (Input.GetKeyDown(KeyCode.Space))
		{
			if (animator.GetBool("Climbing"))
				JumpOff();
			else
				TryClimb(centerOfRaccoon);

		}

		else if (animator.GetBool("Climbing"))
		{
			FaceTowardsClimb();
			CheckIfShouldUnClimb(); // FIX THIS ALEXI
		}
	}
	void CheckIfShouldUnClimb()
	{
		Vector3 topCenter = centerOfRaccoon + Vector3.up / 2.5f;
		Debug.DrawLine(topCenter, topCenter + transform.forward * climbingHorizontalDistance);

		// If not hit a climbable, Climb over
		if (!Physics.Raycast(topCenter, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
		{
			animator.SetBool("Climbing", false);
			Invoke(nameof(EnableWalkParams), 1f);
		}
	}
	void FaceTowardsClimb()
	{
		if (Physics.Raycast(centerOfRaccoon, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
			{
				animator.SetBool("Climbing", true);
				// Get the normal and rotate towards it
				Vector3 forwardDirection = -hit.normal; // Opposite of normal to align correctly

				// forwardDirection.y = 0; // Keep rotation only on Y-axis
				Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
			}
	}
	void JumpOff()
	{
		animator.SetBool("Climbing", false);
		animator.SetTrigger("Jump");
		CancelInvoke();
		Invoke(nameof(EnableWalkParams), 0.65f);
		
	}
	void ResetRotationXZ()
    {
        // Capture the current local rotation
        Vector3 currentRotation = transform.localEulerAngles;

        // Define the target rotation with x and z set to 0, preserving the current y rotation
        Vector3 targetRotation = new Vector3(0f, currentRotation.y, 0f);

        // Smoothly interpolate to the target rotation over the specified duration
        transform.DOLocalRotate(targetRotation, 0.2f, RotateMode.Fast);
    }
	void EnableWalkParams()
	{
		rb.useGravity = true;
		walkingCollider.enabled = true;
		ResetRotationXZ();
	} 
	void TryClimb(Vector3 centerOfRaccoon)
	{
		if (Physics.Raycast(centerOfRaccoon, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
		{
			walkingCollider.enabled = false;
			rb.useGravity = false;
			animator.SetBool("Climbing", true);
			transform.DOMove(transform.position + transform.forward / 4, 1f);
		}
	}
}
