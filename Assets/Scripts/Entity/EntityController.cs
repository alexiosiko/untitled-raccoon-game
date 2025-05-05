using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EntityFootsteps))]
public class EntityController : Interactable
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
	[HideInInspector] public bool isRunning = false;
    public Transform destinationTransform;
    float smoothHorizontal = 0f;
    float smoothVertical = 0f;
    protected AudioSource source;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;

        agent.updateRotation = false;
    }


	public void FollowDestination()
	{
		if (destinationTransform != null)
			agent.SetDestination(destinationTransform.position);
		
		if (agent.hasPath == false)
			return;

		
		// Determine the next corner to navigate towards
		var corners = agent.path.corners;
		Vector3 next = (corners.Length > 1) ? corners[1] : corners[0];
		Vector3 toNext = (next - transform.position).normalized;
		Vector3 localDir = transform.InverseTransformDirection(toNext);

		// Set maximum values based on running state
		float maxForward = isRunning ? 2f : 1f;
		float maxHorizontal = isRunning ? 2f : 1f;

		// Smoothly interpolate horizontal movement
		smoothHorizontal = Mathf.Lerp(
			smoothHorizontal,
			localDir.x * maxHorizontal,
			Time.deltaTime * agent.angularSpeed
		);

		// Calculate forward movement based on agent velocity
		Vector3 localVelRaw = transform.InverseTransformDirection(agent.velocity);
		float forwardInput = Mathf.Clamp(localVelRaw.z / agent.speed, -1f, 1f);
		float acceleration = 5f;
		smoothVertical = Mathf.Lerp(
			smoothVertical,
			forwardInput * maxForward,
			Time.deltaTime * acceleration
		);

		// Apply the calculated values to the animator
		animator.SetFloat("Left", smoothHorizontal);
		animator.SetFloat("Forward", smoothVertical);
	}



    void OnAnimatorMove()
    {
        // Apply root rotation and position from animations
        transform.rotation = animator.rootRotation;
        transform.position = agent.nextPosition;
        
        // Get root motion movement from the animator
        Vector3 rootMotionPosition = animator.deltaPosition;
        
        // Move the character controller using root motion
        agent.nextPosition = transform.position + rootMotionPosition;
    }

    public override void Action(MonoBehaviour caller) {}
}
