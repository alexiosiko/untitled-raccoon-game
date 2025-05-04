using System.Collections;
using UnityEngine;

public class RaccoonFallingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    public RaccoonFallingState(RaccoonStateMachine machine) : base(RaccoonState.Falling) => this.machine = machine;

    public override void EnterState()
    {
		machine.rb.useGravity = true;
		machine.animator.applyRootMotion = false;
		machine.walkingCollider.enabled = true;
		machine.animator.CrossFade("Falling", 0.25f);
		// machine.ForwardForce();
    }

    public override void UpdateState()
    {
    }

    public override RaccoonState GetNextState()
	{
        if (CanLand())
          	return RaccoonState.Landing;
		else
			return StateKey;
	}
	bool CanLand()
	{
		Vector3 startPos = machine.controller.centerOfRaccoon ;
		Vector3 boxHalfExtents = new Vector3(0.25f, 0.05f, 0.3f); // Wide but flat box
		LayerMask entityLayer = LayerMask.GetMask("Entity");
		float distance = 0.4f;
		#if UNITY_EDITOR
		// Draw the box cast area (for debugging)
		Debug.DrawLine(startPos, startPos + Vector3.down * distance, Color.black, 0.5f);
		// Optional: Draw the box itself
		CustomDebug.DrawBox(startPos + Vector3.down * distance, boxHalfExtents, Quaternion.identity, Color.black);
		#endif
		return Physics.BoxCast(startPos, boxHalfExtents, Vector3.down, Quaternion.identity, distance, ~entityLayer);
	}



    public override IEnumerator ExitState()
    {
		Debug.Log("Falling done");
		machine.animator.applyRootMotion = true;
		machine.animator.CrossFade("Landing", 0.25f);
        machine.walkingCollider.enabled = true;

		yield return null;
	}
}