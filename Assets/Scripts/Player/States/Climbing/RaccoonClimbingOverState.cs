using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class RaccoonClimbingOverState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingOverState(RaccoonStateMachine machine) : base(RaccoonState.ClimbingOver)
	{
		this.machine = machine;
	}
	
	public override IEnumerator EnterState()
	{
		machine.animator.CrossFade("Climb Over", 0.25f);
		if (RaccoonClimbingDownState.CanClimbDown(machine, 2f))
		{
			machine.SetState(RaccoonState.ClimbingDown, 1f);
			Debug.Log("Climbing down action sent");
		}
		else
		{

			machine.SetState(RaccoonState.Walking, 1.2f);
			Debug.Log("Walking action sent");

		}
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