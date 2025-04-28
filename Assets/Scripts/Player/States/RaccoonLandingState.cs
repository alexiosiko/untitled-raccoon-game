public class RaccoonLandingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
	public RaccoonLandingState(RaccoonStateMachine machine) : base(RaccoonState.Landing)
	{
		this.machine = machine;
	}

	public override void EnterState()
	{
		machine.animator.CrossFade("Landing", 0.1f);
		machine.SetState(RaccoonState.Walking, 1f);	
	}

	public override void ExitState()
	{
		machine.controller.smoothLeft = 0;
		machine.controller.smoothForward = 0;
	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
}