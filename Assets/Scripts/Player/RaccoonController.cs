using DG.Tweening;
using UnityEngine;
enum RaccoonState
{	
	Carrying,
	JumpOff,
	ClimbOver,
	Falling,
	Jumping,
	Walking,
    Climbing
}
[RequireComponent(typeof(EntityFootsteps))]
[RequireComponent(typeof(Animator))]
public class RaccoonController : MonoBehaviour
{
	void LateUpdate()
    {
        UpdateInput();
		UpdateParams();

		switch (currentState)
		{
			case RaccoonState.Falling: 		HandleFalling(); break;
			case RaccoonState.Jumping: 		HandleJumping(); break;
			case RaccoonState.Walking: 		HandleWalking(); break;
			case RaccoonState.Climbing: 	HandleClimbing(); break;
		}
	}
	void HandleFalling()
	{
		if (animator.GetBool("isGrounded"))
			SetState(RaccoonState.Walking);
	}
	void HandleJumping()
	{
		if (animator.GetBool("isGrounded"))
			SetState(RaccoonState.Walking);
	}

	void SetState(RaccoonState state)
	{
		currentState = state;
		switch (state)
		{
			case RaccoonState.JumpOff:
				animator.SetBool("Climbing", false);
				animator.SetTrigger("Jump");
				ResetRotationXZ();
				Invoke(nameof(SetWalkingState), 1f);
				break;
			case RaccoonState.ClimbOver:
				Invoke(nameof(SetWalkingState), 1.25f);
				animator.SetTrigger("ClimbOver");
				animator.SetBool("Climbing", false);
				break;
			case RaccoonState.Falling:
				break;
			case RaccoonState.Walking:
				walkingCollider.enabled = true;
				ResetRotationXZ();
				rb.useGravity = true;
				break;
			case RaccoonState.Climbing:
				startClimbingDelay = true;
				Invoke(nameof(StopClimbingDelay), 1.5f);
				Invoke(nameof(DisableCollider), 0.25f);
				rb.useGravity = false;
				animator.SetBool("Climbing", true);

				// Hover from climbable
				if (Physics.Raycast(centerOfRaccoon, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
				{
					Vector3 newPos = hit.point + Vector3.down / 10f + hit.normal / 5f;
					transform.DOMove(newPos, 0.75f);
				}
				
				break;
			case RaccoonState.Jumping:
				Invoke(nameof(EnableGravity), 0.5f);
				animator.SetTrigger("Jump");
				break;
		}
	}
	void HandleWalking()
	{
		Debug.DrawLine(centerOfRaccoon, transform.position + Vector3.up / 4f + transform.forward * climbingHorizontalDistance, Color.magenta);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (CanClimb())
				SetState(RaccoonState.Climbing);
			else
				SetState(RaccoonState.Jumping);
		}
			
	}
	void HandleClimbing()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SetState(RaccoonState.JumpOff);
			return;
		}

		PositionAndRotateClimb();
		if (!startClimbingDelay)
			CheckClimbOver();
	}
	bool startClimbingDelay = false;
	void StopClimbingDelay() => startClimbingDelay = false;
	void UpdateParams()
	{
		centerOfRaccoon = transform.position + Vector3.up / 4 + transform.forward / 4f;

		UpdateAnimatorParameters();
        UpdateGroundedStatus();
	}

	void UpdateGroundedStatus()
	{
		bool isGrounded;


		float rayLength = 0.4f; // Adjust as needed
		RaycastHit hit;
		if (Physics.Raycast(centerOfRaccoon, Vector3.down, out hit, rayLength)) 
			isGrounded = true;
		else
			isGrounded = false;

		animator.SetBool("isGrounded", isGrounded);
		
		// Debugging: Draw the box and contact point
		Debug.DrawLine(centerOfRaccoon, centerOfRaccoon + Vector3.down * rayLength, Color.red);
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
		Vector3 topCenterAndBack = transform.position + Vector3.up / 1.25f;
		Debug.DrawLine(topCenterAndBack, topCenterAndBack + transform.forward * climbingHorizontalDistance / 1.2f, Color.cyan);

		// If not hit a climbable, Climb over
		if (!Physics.Raycast(topCenterAndBack, transform.forward, out RaycastHit hit, climbingHorizontalDistance / 1.2f, LayerMask.GetMask("Climbable")))
			SetState(RaccoonState.ClimbOver);
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

			
			// transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 4);
		}
	}
	void ResetRotationXZ()
    {
        // Capture the current local rotation
        Vector3 currentRotation = transform.localEulerAngles;

        // Define the target rotation with x and z set to 0, preserving the current y rotation
        Vector3 targetRotation = new(0f, currentRotation.y, 0f);

        // Smoothly interpolate to the target rotation over the specified duration
        transform.DOLocalRotate(targetRotation, 0.2f, RotateMode.Fast);
    }

	void Awake()
    {
        animator = GetComponent<Animator>();
        walkingCollider = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
    }
	bool CanClimb() => Physics.Raycast(centerOfRaccoon, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable"));
	void EnableGravity() => rb.useGravity = true;
	void SetWalkingState() => SetState(RaccoonState.Walking);
	void DisableCollider() => walkingCollider.enabled = false;
	[SerializeField] RaccoonState currentState;
    float smoothHorizontal = 0f;
   	float smoothVertical = 0f;
    float inputSmoothingSpeed = 5f;
    [SerializeField] float groundCheckDistance = 0.1f;
    private Animator animator;
	private Collider walkingCollider;
	float climbingHorizontalDistance = 0.8f;
	Rigidbody rb;
	Vector3 centerOfRaccoon;
}
