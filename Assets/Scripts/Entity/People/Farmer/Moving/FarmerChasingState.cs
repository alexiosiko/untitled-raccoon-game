using System.Collections;
using UnityEngine;

public class FarmerChasingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerChasingState(FarmerStateMachine machine) : base(FarmerState.Chasing) => this.machine = machine;
	public override void EnterState()
	{
		machine.isRunning = true;
		machine.animator.CrossFade("Walking", 0.2f);
		machine.destinationTransform = GameObject.Find("Raccoon").transform;
		machine.agent.isStopped = false;
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
		if (machine.agent.remainingDistance < 1f)
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