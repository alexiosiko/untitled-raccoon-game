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
		machine.rb.useGravity = true;
		machine.animator.applyRootMotion = false;
		machine.animator.CrossFade("Falling", 0.25f);
		// machine.ForwardForce();
    }

    public override void UpdateState()
    {
    }

    public override RaccoonState GetNextState()
	{
        if (machine.animator.GetBool("IsGrounded"))
          	return RaccoonState.Landing;
		else
			return StateKey;
	}



    public override void ExitState()
    {
		machine.animator.applyRootMotion = true;
		machine.animator.CrossFade("Landing", 0.25f);
        machine.walkingCollider.enabled = true;
		machine.controller.smoothLeft = 0;
		machine.controller.smoothForward = 0;
		
	}
}