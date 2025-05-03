using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class RaccoonGrabbingState : BaseState<RaccoonState>
{
	readonly RaccoonStateMachine machine;
	public static Grabable grabable;
	bool delay = false;
	public RaccoonGrabbingState(RaccoonStateMachine machine) : base(RaccoonState.Grabbing) => this.machine = machine;
	public override void EnterState()
	{
		delay = true;
		machine.StartCoroutine(RemoveDelay());
		machine.animator.CrossFade("Grabbing", 0.5f);

		grabable.SetGrabState(machine);
	}
	IEnumerator RemoveDelay()
	{
		yield return new WaitForSeconds(1);
		delay = false;
	}
	public override IEnumerator ExitState()
	{
		grabable.SetDropState(machine);
		grabable.GetComponent<Rigidbody>().AddForce(machine.transform.forward / 2f);
		machine.animator.CrossFade("Walking", 0.75f);
		yield return null;
	}

	public override RaccoonState GetNextState()
	{
		if (!delay && Input.GetKeyDown(KeyCode.Space))
			return RaccoonState.Walking;
		return StateKey;
	}
	public static bool CanGrab(RaccoonStateMachine machine)
	{
		Vector3 pos = machine.controller.centerOfRaccoon + machine.transform.forward / 2f;
		float radius = 0.5f;
		Collider[] colliders = Physics.OverlapSphere(pos, radius );
		CustomDebug.DebugSphere(pos, radius, Quaternion.identity, Color.green, radius );
		foreach (var c in colliders)
		{
			Grabable i = c.GetComponentInParent<Grabable>();
			if (i)
			{
				i.Action(machine);
				grabable = i;
				return true;
			}

		}
		return false;
	}
	public override void UpdateState()
	{
	}
}