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
		
		machine.Invoke("SetWalkingState", 1.5f);
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