using System.Collections;
using UnityEngine;

public class RaccoonClimbingDownState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingDownState(RaccoonStateMachine machine) : base(RaccoonState.Climbing)
	{
		this.machine = machine;
	}
	
	public override IEnumerator EnterState()
	{
		machine.walkingCollider.enabled = false;
		machine.rb.useGravity = false;
		machine.animator.CrossFade("Climb Down", 0.25f);

		// Make sure this length is the correct length of the animation clip
		yield return new WaitForSeconds(0.5f);
		machine.SetState(RaccoonState.Falling);
	}

	public override IEnumerator ExitState()
	{
		machine.walkingCollider.enabled = true;
		yield return null;
	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
	public static bool CanClimbDown(RaccoonStateMachine machine)
	{
		
		Vector3 forwardPosAndUp = machine.controller.centerOfRaccoon + machine.transform.forward / 1f + machine.transform.up / 4f;
		Vector3 boxHalfExtents = new Vector3(0.15f, 0.05f, 0.4f); // Wide but flat box
		LayerMask entityLayer = LayerMask.GetMask("Entity");
		float distance = 1f;
		#if UNITY_EDITOR
		Debug.DrawLine(forwardPosAndUp, forwardPosAndUp + Vector3.down * distance, Color.white, 0.1f);
		CustomDebug.DrawBox(forwardPosAndUp + Vector3.down * distance, boxHalfExtents, machine.transform.rotation, Color.cyan, 0.1f);
		#endif
		return !Physics.BoxCast(forwardPosAndUp, boxHalfExtents, Vector3.down, machine.transform.rotation, distance, ~entityLayer);

		
	}
}