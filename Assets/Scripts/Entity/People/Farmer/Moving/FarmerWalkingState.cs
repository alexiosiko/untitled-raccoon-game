using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FarmerWalkingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerWalkingState(FarmerStateMachine machine) : base(FarmerState.Walking) => this.machine = machine;
	public override void EnterState()
	{
		// machine.StartCoroutine(UnDelay());
		machine.agent.isStopped = false;
		machine.animator.CrossFade("Walking", 0.25f);
	}

	public override IEnumerator ExitState()
	{
		machine.agent.isStopped = true;
		yield return null;
	}

	public override FarmerState GetNextState()
	{
		Debug.Log(machine.agent.pathPending + " : " + machine.agent.destination);
		if (!machine.agent.pathPending && machine.agent.remainingDistance < 1)
			return FarmerState.Planting;

		// foreach (var r in machine.resettableObjects)
		// {
		// 	if (r.needsToBeReset && !r.isGrabbed)
		// 	{
		// 		machine.destinationTransform = r.transform;
		// 	 	return FarmerState.Chasing;
		// 	}
		// }

		
		return StateKey;
	}
	// bool delay = false;
	// IEnumerator UnDelay()
	// {
	// 	yield return new WaitForSeconds(2f);
	// 	delay = false;
	// }

	public override void UpdateState()
	{
		machine.FollowDestination();

	}
}