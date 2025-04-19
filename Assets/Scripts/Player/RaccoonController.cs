using UnityEngine;

public class RaccoonController : MonoBehaviour
{
	void Update()
	{
		UpdateInput();
		UpdateAnimatorParameters();
		centerOfRaccoon = transform.position + Vector3.up / 4 + transform.forward / 4f;
	}
	public bool IsGrounded()
	{
		// Define ground check parameters
		Vector3 boxCenter = centerOfRaccoon + Vector3.down * 0f; // Slightly below raccoon
		Vector3 boxHalfExtents = new Vector3(0.25f, 0.05f, 0.7f); // Wide but flat box
		float maxDistance = 0.4f; // How far to check downward
		
		// Perform the box cast
		bool isGrounded = Physics.BoxCast(
			boxCenter, 
			boxHalfExtents, 
			Vector3.down, 
			Quaternion.identity, 
			maxDistance, 
			~LayerMask.GetMask("Entity") // Exclude Entity layer
		);
		
		#if UNITY_EDITOR
		Debug.DrawRay(boxCenter, Vector3.down * maxDistance, isGrounded ? Color.green : Color.red);
		#endif
		
		return isGrounded;
	}
	void UpdateInput()
	{
		float rawHorizontal = Input.GetAxis("Horizontal");
		float rawVertical = Input.GetAxis("Vertical");
		if (Input.GetKey(KeyCode.Space)) // Space to walk
		{
			rawHorizontal *= 1 + rawVertical;
			rawVertical *= 2f;
		}


		smoothLeft = Mathf.Lerp(smoothLeft, rawHorizontal,  Time.deltaTime * 10f);
		smoothForward = Mathf.Lerp(smoothForward, rawVertical, Time.deltaTime * 3f);
	}
	void UpdateAnimatorParameters()
	{
		animator.SetFloat("Left", smoothLeft);
		animator.SetFloat("Forward", smoothForward);

		animator.SetBool("IsGrounded", IsGrounded());
	}
	
	void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		walkingCollider = GetComponent<Collider>();
	}
	public void ForceWalk()
	{
		smoothLeft = Mathf.Clamp(smoothLeft, -1f, 1f);
		smoothForward = Mathf.Clamp(smoothForward, -2f, 1f);
	}
	[HideInInspector] public float smoothLeft;
	[HideInInspector] public float smoothForward;
	[HideInInspector] public Vector3 centerOfRaccoon;
	[HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody rb;
	[HideInInspector] public Collider walkingCollider;
	Animator animator;
}