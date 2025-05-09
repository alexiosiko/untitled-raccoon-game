using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FarmerCarryingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerCarryingState(FarmerStateMachine machine) : base(FarmerState.Carrying) => this.machine = machine;
	

	public override void EnterState()
	{
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