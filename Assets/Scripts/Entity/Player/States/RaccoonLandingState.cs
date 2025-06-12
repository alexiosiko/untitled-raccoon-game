using System.Collections;
using UnityEngine;

public class RaccoonLandingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
	public RaccoonLandingState(RaccoonStateMachine machine) : base(RaccoonState.Landing)
	{
		this.machine = machine;
	}

	public override IEnumerator EnterState()
	{
		machine.animator.CrossFade("Landing", 0.1f);
		yield return new WaitForSeconds(0.5f);
		machine.SetState(RaccoonState.Walking);	
		yield return null;
	}

	public override IEnumerator ExitState()
	{
		machine.controller.smoothLeft = 0;
		machine.controller.smoothForward = 0;
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