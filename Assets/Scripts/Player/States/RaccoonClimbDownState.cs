using Unity.VisualScripting;
using UnityEngine;

public class RaccoonClimbingDownState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingDownState(RaccoonStateMachine machine) : base(RaccoonState.Climbing)
	{
		this.machine = machine;
	}
	
	public override void EnterState()
	{
		// Make sure this length is the correct length of the animation clip
		float animationLength = 0.5f;
		machine.Invoke(nameof(machine.SetFallingState), animationLength);
	}

	public override void ExitState()
	{
		machine.ForwardForce();
	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
}