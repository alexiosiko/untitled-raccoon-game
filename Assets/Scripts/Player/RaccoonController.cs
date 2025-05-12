using Unity.Mathematics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class RaccoonController : MonoBehaviour
{
	public static Vector3 halfExtends = new Vector3(0.2f, 0.05f, 0.3f);
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
		Vector3 boxHalfExtents = halfExtends; // Wide but flat box
		float maxDistance = 0.4f; // How far to check downward
		Quaternion orientation = transform.rotation;
		// Perform the box cast
		bool isGrounded = Physics.BoxCast(
			boxCenter, 
			boxHalfExtents, 
			Vector3.down, 
			orientation, 
			maxDistance, 
			~LayerMask.GetMask("Entity") // Exclude Entity layer
		);
		
		#if UNITY_EDITOR
		Debug.DrawLine(boxCenter, boxCenter +  Vector3.down * maxDistance);
		CustomDebug.DrawBox(boxCenter + Vector3.down * maxDistance, boxHalfExtents, orientation, isGrounded ? Color.green : Color.red);
		#endif
		
		return isGrounded;
	}
	void UpdateInput()
	{
		float rawHorizontal = Input.GetAxis("Horizontal");
		float rawVertical = Input.GetAxis("Vertical");
		if (!Input.GetKey(KeyCode.Space)) // Space to walk
		{
			rawHorizontal *= 2;
			rawVertical *= 2f;
		}
		else
		{
			if (rawVertical < 0)
				rawVertical*= 2f;
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
		smoothForward = Mathf.Clamp(smoothForward, -1f, 1f);
	}
	[HideInInspector] public float smoothLeft;
	[HideInInspector] public float smoothForward;
	[HideInInspector] public Vector3 centerOfRaccoon;
	[HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody rb;
	[HideInInspector] public Collider walkingCollider;
	Animator animator;
}