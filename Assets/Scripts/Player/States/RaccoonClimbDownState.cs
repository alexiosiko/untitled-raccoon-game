public class RaccoonClimbingDownState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingDownState(RaccoonStateMachine machine) : base(RaccoonState.Climbing)
	{
		this.machine = machine;
	}
	
	public override void EnterState()
	{
		machine.Invoke("SetWalkingState", 1.25f);
	}

	public override void ExitState()
	{
		machine.ForwardForce();
	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
}