using DG.Tweening;
using UnityEngine;

public enum RaccoonState
{	
	Carrying,
	Falling,
	JumpOff,
	Walking,
	Climbing,
	ClimbingOver,
	ClimbingDown,
}

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(Collider))]
public class RaccoonStateMachine : StateMachine<RaccoonState>
{
	[SerializeField] string currentState;
    // Public properties for states to access
    public Animator animator;
    public Rigidbody rb;
    public Collider walkingCollider;
    public Vector3 centerOfRaccoon;
    
    // Movement parameters
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Vector3 MoveInput { get; private set; }
    void LateUpdate()
    {
		base.Update();
		currentState = CurrentState.ToString();

        // Update center point for raycasts
        centerOfRaccoon = transform.position + Vector3.up / 4 + transform.forward / 4f;
        
		UpdateInput();
		UpdateAnimatorParameters();

		if (Input.GetKeyDown(KeyCode.F))
			ForwardForce();
    }

    public void ResetRotationXZ()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        Vector3 targetRotation = new(0f, currentRotation.y, 0f);
        transform.DOLocalRotate(targetRotation, 0.2f, RotateMode.Fast);
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
		smoothHorizontal = Mathf.Lerp(smoothHorizontal, rawHorizontal,  Time.deltaTime * 5f);
		smoothVertical = Mathf.Lerp(smoothVertical, rawVertical, Time.deltaTime * 2f);
	}
	void UpdateAnimatorParameters()
	{
		animator.SetFloat("Left", smoothHorizontal);
		animator.SetFloat("Forward", smoothVertical);

		animator.SetBool("IsGrounded", IsGrounded());
	}
	public void ForwardForce() 
	{
		rb.AddForce(transform.forward * 50f);
	}
	
	public bool IsGrounded(float lengthMultiplier = 1)
	{
		float rayLength = 0.4f;
		int layerMask = ~LayerMask.GetMask("Entity"); // Exclude "Entity" layer
		Debug.DrawLine(centerOfRaccoon, centerOfRaccoon + Vector3.down * rayLength, Color.red);
		return Physics.Raycast(centerOfRaccoon, Vector3.down, rayLength, layerMask);
	}


   
	public float smoothHorizontal;
	public float smoothVertical;
	void OnDrawGizmos()
	{
		Vector3 topCenterAndBack = transform.position + Vector3.up / 1f; // This value is also in child;
		Vector3 halfExtents = Vector3.one / 8f;
		Quaternion orientation = Quaternion.identity;
		Vector3 direction = transform.forward * (RaccoonClimbingState.climbingHorizontalDistance / 1.05f);

		Gizmos.color = Color.cyan;
		Gizmos.matrix = Matrix4x4.TRS(topCenterAndBack, orientation, Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero + direction, halfExtents * 2f);
	}
	public override void Awake()
    {
        // Get required components
        walkingCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        // Initialize all states
        States.Add(RaccoonState.Walking, 		new RaccoonWalkingState(this));
        States.Add(RaccoonState.Climbing, 		new RaccoonClimbingState(this));
        States.Add(RaccoonState.ClimbingOver, 	new RaccoonClimbingOverState(this));
        States.Add(RaccoonState.ClimbingDown, 	new RaccoonClimbingDownState(this));
        States.Add(RaccoonState.Falling, 		new RaccoonFallingState(this));
        
        // Set initial state
        TransitionToState(RaccoonState.Walking);
    }
	public void SetWalkingState() => TransitionToState(RaccoonState.Walking);
	public void SetFallingState() => TransitionToState(RaccoonState.Falling);
	public void ApplyRootMotion() => animator.applyRootMotion = true;

}