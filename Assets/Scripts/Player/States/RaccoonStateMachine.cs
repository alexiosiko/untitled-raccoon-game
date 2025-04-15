using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public enum RaccoonState
{
	Biting,
	Eating,	
	Grabbing,
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
    void LateUpdate()
    {
		base.Update();
		currentStateName = CurrentState.ToString();

    }
    public void ResetRotationXZ()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        Vector3 targetRotation = new(0f, currentRotation.y, 0f);
        transform.DOLocalRotate(targetRotation, 0.2f, RotateMode.Fast);
    }
	public void ForwardForce() => rb.AddForce(transform.forward * 30f);
	public override void Awake()
    {
        // Get required components
        walkingCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
		controller = GetComponent<RaccoonController>();
        
        // Initialize all states
        States.Add(RaccoonState.Walking, 		new RaccoonWalkingState(this));
        States.Add(RaccoonState.Climbing, 		new RaccoonClimbingState(this));
        States.Add(RaccoonState.ClimbingOver, 	new RaccoonClimbingOverState(this));
        States.Add(RaccoonState.ClimbingDown, 	new RaccoonClimbingDownState(this));
        States.Add(RaccoonState.Falling, 		new RaccoonFallingState(this));
        States.Add(RaccoonState.Eating, 		new RaccoonEatingState(this));
        States.Add(RaccoonState.Grabbing, 		new RaccoonGrabbingState(this));
        States.Add(RaccoonState.Biting, 		new RaccoonBitingState(this));
        
        // Set initial state
        SetState(RaccoonState.Walking);
    }
	public void SetWalkingState() => SetState(RaccoonState.Walking);
	public void SetFallingState() => SetState(RaccoonState.Falling);
	public void SetEatingState(Interactable consumable)
	{
		var state =  States[RaccoonState.Eating] as RaccoonEatingState;
		state.consumable = consumable;
		SetState(RaccoonState.Eating);
	}
	public void SetGrabbingState(Grabable grabable)
	{
		var state =  States[RaccoonState.Grabbing] as RaccoonGrabbingState;
		state.grabable = grabable;
		SetState(RaccoonState.Grabbing);
	}
	public void ApplyRootMotion() => animator.applyRootMotion = true;


	#region PARAMS
	[SerializeField] string currentStateName;
	public Transform mouthTransform;
	public Transform grabTransform;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider walkingCollider;
    public Collider climbingCollider;
	[HideInInspector] public RaccoonController controller;
	#endregion


}