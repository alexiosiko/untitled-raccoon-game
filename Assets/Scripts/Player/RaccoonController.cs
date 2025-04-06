using DG.Tweening;
using UnityEngine;
enum RaccoonState
{	
	Grabbing,
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
	
	void SetState(RaccoonState state)
	{
		ExitState(); 
		currentState = state;
		switch (state)
		{
			case RaccoonState.JumpOff:
				animator.SetBool("Climbing", false);
				animator.SetTrigger("Jump");
				Invoke(nameof(SetWalkingState), 1f);
				break;
			case RaccoonState.ClimbOver:
				Invoke(nameof(JumpOffForwardPush), 0.2f);
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
				CancelInvoke(nameof(StopClimbingDelay));
				CancelInvoke(nameof(DisableCollider));
				Invoke(nameof(StopClimbingDelay), 2f);
				Invoke(nameof(DisableCollider), 0.25f);
				rb.useGravity = false;
				animator.SetBool("Climbing", true);

				// Hover from climbable
				if (Physics.Raycast(centerOfRaccoon, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
				{
					Vector3 newPos = hit.point + Vector3.down / 10f + hit.normal / 4f;
					transform.DOMove(newPos, 0.75f);
				}
				
				break;
			case RaccoonState.Jumping:
				Invoke(nameof(EnableGravity), 0.5f);
				animator.SetTrigger("Jump");
				break;
		}
	}
	void ExitState()
	{
		// switch (currentState)
		// {

		// }
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
	void HandleWalking()
	{
		Debug.DrawLine(centerOfRaccoon, transform.position + Vector3.up / 4f + transform.forward * climbingHorizontalDistance, Color.magenta);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (CanClimb())
				SetState(RaccoonState.Climbing);
		}
			
	}
	void HandleClimbing()
	{
		if (!startClimbingDelay && Input.GetKeyDown(KeyCode.Space))
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
		if (!Input.GetKey(KeyCode.Space)) // Space to walk
		{
			rawHorizontal *= 1 + rawVertical;
			rawVertical *= 2f;
		}
        smoothHorizontal = Mathf.Lerp(smoothHorizontal, rawHorizontal, inputSmoothingSpeed * Time.deltaTime * 2f);
        smoothVertical = Mathf.Lerp(smoothVertical, rawVertical, inputSmoothingSpeed * Time.deltaTime / 2);
    }

    void UpdateAnimatorParameters()
    {
        animator.SetFloat("Left", smoothHorizontal);
        animator.SetFloat("Forward", smoothVertical);
    }

	void CheckClimbOver()
	{
		Vector3 topCenterAndBack = transform.position + Vector3.up / 1.05f;
		Debug.DrawLine(topCenterAndBack, topCenterAndBack + transform.forward * climbingHorizontalDistance / 1.2f, Color.cyan);
	
	    Vector3 halfExtents = Vector3.one / 8f;

		if (!Physics.BoxCast(topCenterAndBack, halfExtents, transform.forward, Quaternion.identity, climbingHorizontalDistance / 1.2f))
			SetState(RaccoonState.ClimbOver);

	}
	void OnDrawGizmos()
	{
		Vector3 topCenterAndBack = transform.position + Vector3.up / 1.05f;
		Vector3 halfExtents = Vector3.one / 8f;
		Quaternion orientation = Quaternion.identity;
		Vector3 direction = transform.forward * (climbingHorizontalDistance / 1.2f);

		Gizmos.color = Color.cyan;
		Gizmos.matrix = Matrix4x4.TRS(topCenterAndBack, orientation, Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero + direction, halfExtents * 2f);
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

			// Vector3 newPos = hit.point + hit.normal / 2f;
			// transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
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
	bool CanClimb()
	{
		return Physics.Raycast(centerOfRaccoon, transform.forward, out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable"));
	}
	void EnableGravity() => rb.useGravity = true;
	void SetWalkingState() => SetState(RaccoonState.Walking);
	void DisableCollider() => walkingCollider.enabled = false;
	[SerializeField] RaccoonState currentState;
	float smoothHorizontal;
	float smoothVertical;
    float inputSmoothingSpeed = 5f;
    [SerializeField] float groundCheckDistance = 0.1f;
    private Animator animator;
	private Collider walkingCollider;
	float climbingHorizontalDistance = 0.8f;
	Rigidbody rb;
	Vector3 centerOfRaccoon;
	public void JumpOffForwardPush()  
	{
		rb.AddForce(transform.forward * 5f);
	}

}
