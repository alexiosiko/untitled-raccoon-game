using System;
using DG.Tweening;
using UnityEngine;

enum RaccoonState
{
    Walking,
    Climbing
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class RaccoonController : MonoBehaviour
{
	[SerializeField] RaccoonState currentState = RaccoonState.Walking;
	[SerializeField] Transform rigTransform;
    private float smoothHorizontal = 0f;
    private float smoothVertical = 0f;
    public float inputSmoothingSpeed = 5f;
    public float groundCheckDistance = 1f;
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
        UpdateInput();
		UpdateParams();

		switch (currentState)
		{
			case RaccoonState.Walking: 		HandleWalking(); break;
			case RaccoonState.Climbing: 	HandleClimbing(); break;
		}
	}
	void SetState(RaccoonState state)
	{
		currentState = state;
		switch (state)
		{
			case RaccoonState.Walking:
				animator.SetBool("Climbing", false);
				break;
			case RaccoonState.Climbing:
				walkingCollider.enabled = false;
				rb.useGravity = false;

				// Move him a little up so he doesnt climb back down automatically
				animator.SetBool("Climbing", true);
				break;
		}
	}
	void HandleWalking()
	{
		if (Math.Abs(smoothHorizontal) < 0.1f)
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y, 0), Time.deltaTime * 4);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (CanClimb())
				SetState(RaccoonState.Climbing);
			else
				animator.SetTrigger("Jump");
		}
			
	}
	void HandleClimbing()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			JumpOff();

		PositionAndRotateClimb();
		CheckClimbOver();
	}
	void UpdateParams()
	{
		centerOfRaccoon = transform.position + Vector3.up / 4;
		UpdateAnimatorParameters();
        UpdateGroundedStatus();
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

	void CheckClimbOver()
	{
		Vector3 topCenter = centerOfRaccoon + Vector3.up / 2.2f;
		Debug.DrawLine(topCenter, topCenter + transform.forward * climbingHorizontalDistance);

		// If not hit a climbable, Climb over
		if (!Physics.Raycast(topCenter, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
		{
			animator.SetBool("Climbing", false);
			Invoke(nameof(EnableWalkParams), 1f);
		}
	}
	void PositionAndRotateClimb()
	{
		if (Physics.Raycast(centerOfRaccoon, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
		{
			animator.SetBool("Climbing", true);

			// Rotate towards the surface normal
			Vector3 forwardDirection = -hit.normal;
			Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

			
			// Hover from climbable
			float z = hit.point.z + hit.normal.z / 4f;
			Vector3 newPos = new (transform.position.x, transform.position.y, z);
			transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 4);
		}
	}
	void JumpOff()
	{
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
		SetState(RaccoonState.Walking);
		rb.useGravity = true;
		walkingCollider.enabled = true;
		ResetRotationXZ();
	} 
	bool CanClimb() => Physics.Raycast(centerOfRaccoon, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable"));

}
