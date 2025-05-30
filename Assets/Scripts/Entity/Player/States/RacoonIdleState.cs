using System.Collections;

public class RaccoonIdleState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    public RaccoonIdleState(RaccoonStateMachine machine) : base(RaccoonState.Idle) => this.machine = machine;
	public override IEnumerator EnterState()
	{
		machine.animator.CrossFade("Idle", 0.2f);
		yield return null;
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