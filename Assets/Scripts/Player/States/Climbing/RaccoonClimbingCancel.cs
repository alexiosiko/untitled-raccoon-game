using System.Collections;
using DG.Tweening;
using UnityEngine;
public class RaccoonClimbingCancelState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonClimbingCancelState(RaccoonStateMachine machine) : base(RaccoonState.ClimbingCancel)
	{
		this.machine = machine;
	}

	public override void EnterState()
	{
		machine.animator.CrossFade("Climb Cancel", 0.25f);
		// machine.climbingCollider.enabled = false;
		machine.SetState(RaccoonState.Walking, 0.6f);
		machine.StartCoroutine(DoMove());
		
	}
	IEnumerator DoMove()
	{
		yield return new WaitForSeconds(0.2f);
		Vector3 startPos = machine.transform.position + Vector3.up / 2f + -machine.transform.forward / 3f;

		if (Physics.Raycast(startPos, Vector3.down, out RaycastHit hit , 1, ~LayerMask.GetMask("Entity")))
			{
			Vector3 pos = hit.point + Vector3.up / 30f;
			Debug.DrawLine(startPos, pos, Color.magenta, 2);
			machine.transform.DOMove(pos, 0.75f);
		}
	}

	public override IEnumerator ExitState()
	{
		machine.walkingCollider.enabled = true;
		machine.animator.CrossFade("Walking", 0.25f);
		machine.controller.smoothLeft = 0;
		machine.controller.smoothForward = 0;

		yield return new WaitForSeconds(0.5f);

		

	}

	public override RaccoonState GetNextState()
	{
		return RaccoonState.ClimbingCancel;
	}

	public override void UpdateState()
	{
	}
}