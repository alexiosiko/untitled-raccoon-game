using System.Collections;
using UnityEngine;

public class FarmerIdleState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerIdleState(FarmerStateMachine machine) : base(FarmerState.Idle) => this.machine = machine;
	public override IEnumerator EnterState()
	{
		machine.animator.CrossFade("Idle", 0.2f);
		yield return null;
	}
	public override IEnumerator ExitState()
	{
		yield return null;
	}
	public override FarmerState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
}