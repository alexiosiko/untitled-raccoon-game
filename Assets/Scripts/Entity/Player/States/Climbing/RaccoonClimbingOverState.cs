using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class RaccoonClimbingOverState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingOverState(RaccoonStateMachine machine) : base(RaccoonState.ClimbingOver)
	{
		this.machine = machine;
	}

	public override IEnumerator EnterState()
	{
		machine.animator.CrossFade("Climb Over", 0.25f);
		if (CanClimbDownAfterClimbing(machine))
		{
			Debug.Log("enter climbing over");
			yield return new WaitForSeconds(1f);
			machine.SetState(RaccoonState.ClimbingDown, true);
			// Debug.Log("Climbing down action sent");
		}
		else
		{

			yield return new WaitForSeconds(1.2f);
			machine.SetState(RaccoonState.Walking);
			// Debug.Log("Walking action sent");

		}
		yield return null;
	}

	public override IEnumerator ExitState()
	{
		machine.controller.smoothLeft = 0;
		machine.controller.smoothForward = 0;
		yield return null;

	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
	public bool CanClimbDownAfterClimbing(RaccoonStateMachine machine)
	{

		Vector3 forwardPosAndUp = machine.controller.centerOfRaccoon + machine.transform.forward * 1 + machine.transform.up / 1f;
		Vector3 boxHalfExtents = new Vector3(0.25f, 0.05f, 0.4f); // Wide but flat box
		LayerMask entityLayer = LayerMask.GetMask("Entity");
		float distance = 1f;

#if UNITY_EDITOR
		Debug.DrawLine(forwardPosAndUp, forwardPosAndUp + Vector3.down * distance, Color.white, 0.1f);
		CustomDebug.DrawBox(forwardPosAndUp + Vector3.down * distance, boxHalfExtents, machine.transform.rotation, Color.cyan, 0.1f);
#endif

		return !Physics.BoxCast(forwardPosAndUp, boxHalfExtents, Vector3.down, machine.transform.rotation, distance, ~entityLayer);


	}
	public static bool CanClimbOver(RaccoonStateMachine machine, float climbingHorizontalDistance)
	{
		Vector3 topCenterAndBack = machine.transform.position + Vector3.up / 1.2f; // This value is also in parentw
        Vector3 halfExtents = Vector3.one / 8f;
		 float dist = climbingHorizontalDistance / 1.2f;
    Vector3 center = topCenterAndBack + machine.transform.forward * (dist / 2f);


		#if UNITY_EDITOR
		// visualize boxcast start-to-target line
		Debug.DrawLine(topCenterAndBack, topCenterAndBack + machine.transform.forward * dist, Color.white, 0.1f);
			// visualize the cast volume
			CustomDebug.DrawBox(center, halfExtents, machine.transform.rotation, Color.cyan, 0.1f);
			#endif

		return !Physics.BoxCast(topCenterAndBack, halfExtents, machine.transform.forward,
			machine.transform.rotation, climbingHorizontalDistance / 1.2f);
	}
}