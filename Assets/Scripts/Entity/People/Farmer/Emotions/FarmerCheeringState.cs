using System.Collections;

public class FarmerCheeringState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerCheeringState(FarmerStateMachine machine) : base(FarmerState.Cheering)
	{
		this.machine = machine;
	}

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