using System.Collections;
public class GolfChippingState : BaseState<GolfState>
{
	private GolfStateMachine machine;
	public GolfChippingState(GolfStateMachine machine) : base(GolfState.Chipping) => this.machine = machine;

	public override IEnumerator EnterState()
	{
		yield break;
	}

	public override IEnumerator ExitState()
	{
		yield break;

	}

	public override GolfState GetNextState()
	{
		return StateKey;

	}

	public override void UpdateState()
	{
	}
}