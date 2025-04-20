public class RaccoonStartGrabbingState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonStartGrabbingState(RaccoonStateMachine machine) : base(RaccoonState.StartGrabbing)
	{
		this.machine = machine;
	}

	public override void EnterState()
	{
		machine.animator.CrossFade("Start Grabbing", 0.2f);
		machine.SetState(RaccoonState.Grabbing, 0.375f);
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