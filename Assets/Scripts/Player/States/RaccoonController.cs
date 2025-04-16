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
		Vector3 boxCenter = centerOfRaccoon + Vector3.down * 0.1f; // Slightly below raccoon
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
		Debug.DrawRay(boxCenter, Vector3.down * maxDistance, isGrounded ? Color.green : Color.red, 0.1f);
		#endif
		
		return isGrounded;
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
	
	void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		walkingCollider = GetComponent<Collider>();
	}
	[HideInInspector] public float smoothHorizontal;
	[HideInInspector] public float smoothVertical;
	[HideInInspector] public Vector3 centerOfRaccoon;
	[HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody rb;
	[HideInInspector] public Collider walkingCollider;
	Animator animator;
}