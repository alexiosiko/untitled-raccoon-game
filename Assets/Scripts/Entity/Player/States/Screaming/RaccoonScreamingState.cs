using System.Collections;
using UnityEngine;

public class RaccoonScreamingState : BaseState<RaccoonState>
{
	readonly RaccoonStateMachine machine;
	public RaccoonScreamingState(RaccoonStateMachine machine) : base(RaccoonState.Screaming) => this.machine = machine;

	public override IEnumerator EnterState()
	{
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
