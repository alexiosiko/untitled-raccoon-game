using System.Collections;

public class RaccoonDiggingState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonDiggingState(RaccoonStateMachine machine) : base(RaccoonState.Digging)
	{
		this.machine = machine;
	}

	public override void EnterState()
	{
		machine.animator.CrossFade("Digging", 0.2f);
		machine.SetState(RaccoonState.Walking, 2f);
	}

	public override IEnumerator ExitState()
	{
		yield return null;
	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
}