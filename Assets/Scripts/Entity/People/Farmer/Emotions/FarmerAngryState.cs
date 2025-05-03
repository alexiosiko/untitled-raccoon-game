using System.Collections;

public class FarmerAngryState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerAngryState(FarmerStateMachine machine) : base(FarmerState.Angry)
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