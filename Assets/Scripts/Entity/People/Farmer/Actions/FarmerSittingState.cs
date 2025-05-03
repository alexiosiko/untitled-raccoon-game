using System.Collections;
using UnityEngine;

public class FarmerSittingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerSittingState(FarmerStateMachine machine) : base(FarmerState.Sitting)
	{
		this.machine = machine;
	}

	public override void EnterState()
	{
		machine.animator.CrossFade("Start Sit", 0.2f);
	}

	public override IEnumerator ExitState()
	{
		machine.animator.CrossFade("End Sit", 0.2f);
		yield return new WaitForSeconds(1.2f);
	}

	public override FarmerState GetNextState()
	{

		return StateKey;
	}

	public override void UpdateState()
	{
	}
}