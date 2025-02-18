using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AIAnimationController : MonoBehaviour
{
	[SerializeField] Transform destinationTransform;
	[SerializeField] NavMeshAgent agent;
	[SerializeField] Animator animator;
    void Update()
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
}
