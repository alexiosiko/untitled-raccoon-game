using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EntityFootsteps))]
public class EntityController : Interactable
{
    public Transform destinationTransform;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;

    private float smoothHorizontal = 0f;
    private float smoothVertical = 0f;
    protected AudioSource source;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    protected virtual void Update()
    {
        if (destinationTransform)
            agent.SetDestination(destinationTransform.position);
        else
            return;

        var localVelocity = transform.InverseTransformDirection(agent.velocity);

        smoothHorizontal = Mathf.Lerp(smoothHorizontal, localVelocity.x, Time.deltaTime * 3f);
        smoothVertical = Mathf.Lerp(smoothVertical, localVelocity.z, Time.deltaTime * 3f);

        animator.SetFloat("Left", smoothHorizontal);   // Left-right movement
        animator.SetFloat("Forward", smoothVertical);  // Forward movement
    }

    void OnAnimatorMove()
    {
        transform.rotation = animator.rootRotation;
        transform.position = agent.nextPosition;
        agent.nextPosition = transform.position;
    }

    public override void Action(MonoBehaviour caller) {}
}
