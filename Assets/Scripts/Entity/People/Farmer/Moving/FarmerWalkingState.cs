using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FarmerWalkingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerWalkingState(FarmerStateMachine machine) : base(FarmerState.Walking) => this.machine = machine;
	public override IEnumerator EnterState()
	{
		// machine.StartCoroutine(UnDelay());
		FarmerPlantingState.SetRandomPlantDesination(machine);
		machine.agent.isStopped = false;
		machine.animator.CrossFade("Walking", 0.25f);
		yield return null;
	}

	public override IEnumerator ExitState()
	{
		machine.agent.isStopped = true;
		yield return null;
	}

	public override FarmerState GetNextState()
	{
		if (!machine.agent.pathPending && machine.destinationTransform != null && machine.agent.remainingDistance < 1)
		{
			var r = machine.destinationTransform.GetComponent<ResettableObject>();
			if (r)
			{
				machine.SetDestination(r.transform);
				return FarmerState.Carrying;
			}

			return FarmerState.Planting;
		}

		foreach (var r in machine.resettableObjects)
		{
			if (r.needsToBeReset && !r.isGrabbed)
			{
				machine.SetDestination(r.transform);
			 	return FarmerState.Chasing;
			}
		}

		
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