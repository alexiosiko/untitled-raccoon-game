public class RaccoonClimbingOverState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingOverState(RaccoonStateMachine machine) : base(RaccoonState.ClimbingOver)
	{
		this.machine = machine;
	}
	
	public override void EnterState()
	{
		machine.animator.SetTrigger("ClimbOver");
		machine.animator.SetBool("Climbing", false);
		
		machine.Invoke(nameof(machine.SetWalkingState), 1.1f);
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