public class RaccoonDiggingState : BaseState<RaccoonState>
{
	public RaccoonDiggingState(RaccoonState key) : base(key)
	{
	}

	public override void EnterState()
	{
	}

	public override void ExitState()
	{
	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
}