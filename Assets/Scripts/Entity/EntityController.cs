using System.Collections;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EntityFootsteps))]
public class EntityController : Interactable
{
	public bool freeze = false;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
	public bool isRunning = false;
	public Transform destinationTransform { get; private set; }

    float smoothHorizontal = 0f;
    float smoothVertical = 0f;
    protected AudioSource source;


	public void SetDestination(Transform destination)
	{
		destinationTransform = destination;
		agent.SetDestination(destination.position);
	}
	public void SetDestination(Vector3 position)
	{
		destinationTransform = null;
		agent.SetDestination(position);
	}

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

		if (!agent.hasPath)
			return;
		
		// Determine the next corner to navigate towards
		var corners = agent.path.corners;
		if (corners == null || corners.Length == 0)
			return;
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
		// Debug.Log();
		// // Calculate forward movement based on agent velocity
		// Vector3 localVelRaw = transform.InverseTransformDirection(agent.desiredVelocity);
		float forwardInput = Mathf.Clamp(localDir.z / agent.speed, -1f, 1f);

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
		if (freeze)
			return;
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
