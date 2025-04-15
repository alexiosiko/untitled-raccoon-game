using UnityEngine;

public class RaccoonClimbingDownState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingDownState(RaccoonStateMachine machine) : base(RaccoonState.Climbing)
	{
		this.machine = machine;
	}
	
	public override void EnterState()
	{
		machine.walkingCollider.enabled = false;
		// Make sure this length is the correct length of the animation clip
		float animationLength = 0.45f;
		machine.animator.CrossFade("Climb Down", 0.25f);
		machine.Invoke(nameof(machine.SetFallingState), animationLength);
	}

	public override void ExitState()
	{
		machine.walkingCollider.enabled = true;
	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
	public bool CanClimbDown()
	{
		
		Vector3 forwardPosAndUp = machine.controller.centerOfRaccoon + machine.transform.forward + machine.transform.up / 4f;
		Vector3 boxHalfExtents = new Vector3(0.25f, 0.05f, 0.4f); // Wide but flat box
		float distance = 1f;
		#if UNITY_EDITOR
		Debug.DrawLine(forwardPosAndUp, forwardPosAndUp + Vector3.down * distance, Color.white, 1f);
		#endif
		Debug.Log(Physics.BoxCast(forwardPosAndUp, boxHalfExtents, Vector3.down, Quaternion.identity, distance));
		return !Physics.BoxCast(forwardPosAndUp, boxHalfExtents, Vector3.down, Quaternion.identity, distance);

		
	}
}