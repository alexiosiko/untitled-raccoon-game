using System.Collections;
using UnityEngine;

public class FarmerChasingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerChasingState(FarmerStateMachine machine) : base(FarmerState.Chasing) => this.machine = machine;
	public override IEnumerator EnterState()
	{
		machine.isRunning = true;
		machine.animator.CrossFade("Walking", 0.2f);
		machine.agent.isStopped = false;
		yield return null;
	}

	public override IEnumerator ExitState()
	{
		// machine.animator.CrossFade("Angry", 0.2f);
		machine.isRunning = false;
		machine.agent.isStopped = true;
		// yield return new WaitForSeconds(2f);
		yield return null;
	}

	public override FarmerState GetNextState()
	{
		// If chasing a resetable object, but needstobereset is off, stop.
		var r = machine.destinationTransform.GetComponent<ResettableObject>();
		if (r & r.needsToBeReset == false)
			return FarmerState.Walking;

		if (r & r.isGrabbed == true)
			return FarmerState.Walking;

		if (machine.destinationTransform != null && !machine.agent.pathPending && machine.agent.hasPath && machine.agent.remainingDistance < 0.5f)
		{
			// var i = machine.destinationTransform.GetComponent<Grabable>();
			// i.SetDropState(machine);
			return FarmerState.Carrying;
		}
		return StateKey;
	}

	public override void UpdateState()
	{
		machine.FollowDestination();
	}
}