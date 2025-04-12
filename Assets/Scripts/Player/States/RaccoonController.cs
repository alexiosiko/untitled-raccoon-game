using UnityEngine;

public class RaccoonController : MonoBehaviour
{
	// void Update()
	// {
		
	// 	UpdateInput();
	// 	UpdateAnimatorParameters();
	// 	UpdateGroundedStatus();
	// }
	// void UpdateInput()
    // {
	// 	  float rawHorizontal = Input.GetAxis("Horizontal");
    //    float rawVertical = Input.GetAxis("Vertical");
	// 	if (!Input.GetKey(KeyCode.Space)) // Space to walk
	// 	{
	// 		rawHorizontal *= 1 + rawVertical;
	// 		rawVertical *= 2f;
	// 	}
    //     smoothHorizontal = Mathf.Lerp(smoothHorizontal, rawHorizontal,  Time.deltaTime * 10f);
    //     smoothVertical = Mathf.Lerp(smoothVertical, rawVertical, Time.deltaTime);
    // }
	// void UpdateAnimatorParameters()
    // {
    //     animator.SetFloat("Left", smoothHorizontal);
    //     animator.SetFloat("Forward", smoothVertical);
    // }
	// void UpdateGroundedStatus()
	// {
	// 	bool isGrounded;


	// 	float rayLength = 0.4f; // Adjust as needed
	// 	RaycastHit hit;
	// 	if (Physics.Raycast(centerOfRaccoon, Vector3.down, out hit, rayLength)) 
	// 		isGrounded = true;
	// 	else
	// 		isGrounded = false;

	// 	animator.SetBool("isGrounded", isGrounded);
		
	// 	// Debugging: Draw the box and contact point
	// 	Debug.DrawLine(centerOfRaccoon, centerOfRaccoon + Vector3.down * rayLength, Color.red);
	// }
	// void Awake()
	// {
	// 	animator = GetComponent<Animator>();
	// 	rb = GetComponent<Rigidbody>();
	// 	walkingCollider = GetComponent<Collider>();
	// }
	// float smoothHorizontal;
	// float smoothVertical;
	// public static Vector3 centerOfRaccoon;
	// public static float climbingHorizontalDistance = f;
	// public Animator Animator;
    // public Rigidbody rb;
	// public Collider walkingCollider;
	// Animator animator;
}