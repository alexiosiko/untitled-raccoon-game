using System.Collections;
public class GolfWalkingState : BaseState<GolfState>
{
	private GolfStateMachine machine;
	public GolfWalkingState(GolfStateMachine machine) : base(GolfState.Walking) => this.machine = machine;

	public override IEnumerator EnterState()
	{
		machine.SetDestination(machine.golfBallTransform);
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
		machine.FollowDestination();
	}
}