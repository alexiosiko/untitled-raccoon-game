using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class FarmerWorkingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerWorkingState(FarmerStateMachine machine) : base(FarmerState.Working) => this.machine = machine;


	public override void EnterState()
	{
		delay = true;
		machine.StartCoroutine(UnDelay());
		machine.SetRandomPlantDesination();
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
		if (!delay && !machine.agent.pathPending && machine.agent.hasPath && machine.agent.remainingDistance < 1f)
			return FarmerState.Planting;
		return StateKey;
	}
	bool delay = false;
	IEnumerator UnDelay()
	{
		yield return new WaitForSeconds(0.5f);
		delay = false;
	}

	public override void UpdateState()
	{
		machine.FollowDestination();

	}
}