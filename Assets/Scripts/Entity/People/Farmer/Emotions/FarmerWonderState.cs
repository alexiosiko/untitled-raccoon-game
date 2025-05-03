using System.Collections;
using UnityEngine;

public class FarmerWonderState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerWonderState(FarmerStateMachine machine) : base(FarmerState.Wonder)
	{
		this.machine = machine;
	}

	public override void EnterState()
	{

	}

	public override IEnumerator ExitState()
	{
		yield return 2f;
	}

	public override FarmerState GetNextState()
	{

		return StateKey;
	}

	public override void UpdateState()
	{
	}
}