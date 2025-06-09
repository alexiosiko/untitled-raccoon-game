using DG.Tweening;
using UnityEngine;

public enum RaccoonState
{
	Idle,
	Digging,
	Landing,
	ClimbingCancel,
	Dragging,
	Eating,
	Grabbing,
	Falling,
	Walking,
	Climbing,
	ClimbingOver,
	ClimbingDown,
	Drinking,
	Screaming,
	Search
}

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(Collider))]
public class RaccoonStateMachine : PlayerStateMachine<RaccoonState>
{

	void LateUpdate()
	{
		base.Update();
		currentStateName = CurrentState?.ToString();

	}
	public void ResetRotationXZ()
	{
		Vector3 currentRotation = transform.localEulerAngles;
		Vector3 targetRotation = new(0f, currentRotation.y, 0f);
		transform.DOLocalRotate(targetRotation, 0.2f, RotateMode.Fast);
	}
	protected void Awake()
	{
		// Get required components
		walkingCollider = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		controller = GetComponent<RaccoonController>();

		// Initialize all states
		States.Add(RaccoonState.Walking,		new RaccoonWalkingState(this));
		States.Add(RaccoonState.Climbing, 		new RaccoonClimbingState(this));
		States.Add(RaccoonState.ClimbingOver, 	new RaccoonClimbingOverState(this));
		States.Add(RaccoonState.ClimbingDown, 	new RaccoonClimbingDownState(this));
		States.Add(RaccoonState.Falling, 		new RaccoonFallingState(this));
		States.Add(RaccoonState.Eating, 		new RaccoonEatingState(this));
		States.Add(RaccoonState.Grabbing, 		new RaccoonGrabbingState(this));
		States.Add(RaccoonState.Dragging, 		new RaccoonDraggingState(this));
		States.Add(RaccoonState.ClimbingCancel, new RaccoonClimbingCancelState(this));
		States.Add(RaccoonState.Landing, 		new RaccoonLandingState(this));
		States.Add(RaccoonState.Idle, 			new RaccoonIdleState(this));
		States.Add(RaccoonState.Digging, 		new RaccoonDiggingState(this));

		// Set initial state
		SetState(RaccoonState.Walking);
	}


	[HideInInspector] public Rigidbody rb;
	[HideInInspector] public Collider walkingCollider;
	// public Collider climbingCollider;
	[HideInInspector] public RaccoonController controller;
	public Transform mouthTransform;
	public Transform grabTransform;
	public AudioClip raccoonScream;


}