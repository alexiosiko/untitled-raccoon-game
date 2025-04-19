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
		machine.rb.useGravity = false;
		machine.animator.CrossFade("Climb Down", 0.25f);

		// Make sure this length is the correct length of the animation clip
		machine.SetState(RaccoonState.Falling, 0.5f);
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
	public static bool CanClimbDown(RaccoonStateMachine machine, float forwardMultiplier = 1f)
	{
		
		Vector3 forwardPosAndUp = machine.controller.centerOfRaccoon + machine.transform.forward / 3f * forwardMultiplier + machine.transform.up / 4f;
		Vector3 boxHalfExtents = new Vector3(0.25f, 0.05f, 0.4f); // Wide but flat box
		LayerMask entityLayer = LayerMask.GetMask("Entity");
		float distance = 1f;
		#if UNITY_EDITOR
		// Draw the box cast area (for debugging)
		Debug.DrawLine(forwardPosAndUp, forwardPosAndUp + Vector3.down * distance, Color.white);
		// Optional: Draw the box itself
		CustomDebug.DebugBox(forwardPosAndUp + Vector3.down * distance, boxHalfExtents, Quaternion.identity, Color.cyan);
		#endif
		return !Physics.BoxCast(forwardPosAndUp, boxHalfExtents, Vector3.down, Quaternion.identity, distance, ~entityLayer);

		
	}
}