using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RaccoonGrabbingState : BaseState<RaccoonState>
{
	readonly RaccoonStateMachine machine;
	public Grabable grabable;
	bool delay = false;
	public RaccoonGrabbingState(RaccoonStateMachine machine) : base(RaccoonState.Grabbing) => this.machine = machine;
	public override void EnterState()
	{
		delay = true;
		machine.StartCoroutine(RemoveDelay());
		machine.animator.CrossFade("Grabbing", 1f);

		grabable.SetGrabState(machine);
	}
	IEnumerator RemoveDelay()
	{
		yield return new WaitForSeconds(1);
		delay = false;
	}
	public override void ExitState()
	{
		grabable.SetDropState(machine);
		grabable.GetComponent<Rigidbody>().AddForce(machine.transform.forward / 2f);
		machine.animator.CrossFade("Walking", 0.25f);
	}

	public override RaccoonState GetNextState()
	{
		if (!delay && Input.GetKeyDown(KeyCode.Space))
			return RaccoonState.Walking;
		return StateKey;
	}
	public static bool CanGrab(RaccoonStateMachine machine)
	{
		Ray ray = new (machine.controller.centerOfRaccoon, machine.transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, 1f))
		{
			Grabable i = hit.collider.GetComponent<Grabable>();
			if (i)
			{
				i.Action(machine);
				return true;
			}

		}
		return false;
	}
	public override void UpdateState()
	{
	}
}