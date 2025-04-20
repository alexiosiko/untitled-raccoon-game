using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RaccoonClimbingOverState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingOverState(RaccoonStateMachine machine) : base(RaccoonState.ClimbingOver)
	{
		this.machine = machine;
	}
	
	public override void EnterState()
	{
		machine.animator.CrossFade("Climb Over", 0.25f);
		machine.SetState(RaccoonState.Walking, 1.3f);
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