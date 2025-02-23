using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EntityFootsteps))]
public class EntityController : Interactable
{
	[HideInInspector] public Transform destinationTransform;
	[HideInInspector] public Animator animator;
	[HideInInspector] public  NavMeshAgent agent;
    protected virtual void Update()
    {
		if (destinationTransform)
			agent.SetDestination(destinationTransform.position);
		
        // Get the agent's velocity in world space
        Vector3 worldVelocity = agent.velocity;

        // Transform the world velocity to local space
        Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);

        // Extract horizontal and vertical components
        float horizontal = localVelocity.x; // Left/Right movement
        float vertical = localVelocity.z;   // Forward/Backward movement

        // Update Animator parameters
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }
	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
	}
	public override void Action() {}
}
