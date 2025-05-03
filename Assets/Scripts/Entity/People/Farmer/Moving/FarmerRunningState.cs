using System.Collections;
using UnityEngine;

public class FarmerRunningState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerRunningState(FarmerStateMachine machine) : base(FarmerState.Running) => this.machine = machine;


	public override void EnterState()
	{
		machine.isRunning = true;
		machine.animator.CrossFade("Walking", 0.2f);
		machine.destinationTransform = GameObject.Find("Raccoon").transform;
		machine.agent.isStopped = false;
	}

	public override IEnumerator ExitState()
	{
		machine.animator.CrossFade("Angry", 0.2f);
		machine.isRunning = false;
		machine.agent.isStopped = true;
		yield return new WaitForSeconds(2f);
	}

	public override FarmerState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
		machine.FollowDestination();
	}
}