using UnityEngine;

public class RaccoonFallingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    public RaccoonFallingState(RaccoonStateMachine machine) : base(RaccoonState.Falling)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
		machine.animator.applyRootMotion = false;
		machine.ForwardForce();
    }

    public override void UpdateState()
    {
    }

    public override RaccoonState GetNextState()
	{
        if (machine.animator.GetBool("IsGrounded"))
		{
          	return RaccoonState.Walking;
		}
		else
			return StateKey;
	}



    public override void ExitState()
    {
		machine.animator.applyRootMotion = true;

        machine.walkingCollider.enabled = true;
		machine.controller.smoothLeft = 0;
		machine.controller.smoothForward = 0;
		
	}
}