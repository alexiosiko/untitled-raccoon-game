using System.Collections;
using Unity.Mathematics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class RaccoonDiggingState : BaseState<RaccoonState>
{
	RaccoonStateMachine machine;
	public RaccoonDiggingState(RaccoonStateMachine machine) : base(RaccoonState.Digging)
	{
		this.machine = machine;
	}

	public override IEnumerator EnterState()
	{
		machine.animator.CrossFade("Digging", 0.2f);
		yield return new WaitForSeconds(1.85f);
	}

	public override IEnumerator ExitState()
	{
		yield return null;
	}

	public override RaccoonState GetNextState()
	{
		return RaccoonState.Walking;
	}

	public override void UpdateState()
	{
	}
	public static bool CanDig(RaccoonStateMachine machine)
	{
		float maxDistance = 0.2f;
		Vector3 center = machine.controller.centerOfRaccoon;
		Vector3 halfExtends = RaccoonController.halfExtends;
		Quaternion rotation = machine.transform.rotation;
		Debug.DrawLine(center, center + Vector3.down * maxDistance, Color.red, 1f);
		CustomDebug.DrawBox(center, halfExtends, rotation, Color.red, 1f);
		return Physics.BoxCast(
			center,
			halfExtends,
			Vector3.down,
			rotation,
			maxDistance,
			LayerMask.GetMask("Diggable")
		);
	}
}