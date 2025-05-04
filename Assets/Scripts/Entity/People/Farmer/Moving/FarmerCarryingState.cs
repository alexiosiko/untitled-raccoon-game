using System.Collections;
using UnityEngine;

public class FarmerCarryingState : BaseState<FarmerState>
{
	public Transform carryItemTransform;
	private FarmerStateMachine machine;
	public FarmerCarryingState(FarmerStateMachine machine) : base(FarmerState.Carrying) => this.machine = machine;
	public override void EnterState()
	{
		carryItemTransform = machine.destinationTransform;


		var s = carryItemTransform.GetComponent<RaccoonStateMachine>();
		if (s)
		{
			s.SetState(RaccoonState.Idle);
			s.GetComponent<Rigidbody>().isKinematic = true;
		}

		machine.agent.isStopped = false;
		machine.animator.CrossFade("Start Carry", 0.2f);
		machine.destinationTransform = GameObject.Find("Throw Away Transform").transform;
	}

	public override IEnumerator ExitState()
	{
		var s = carryItemTransform.GetComponent<RaccoonStateMachine>();
		if (s)
		{
			s.SetState(RaccoonState.Falling);
			s.GetComponent<Rigidbody>().isKinematic = false;
		}
		yield return null;
	}

	

	public override FarmerState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
		carryItemTransform.position = Vector3.Lerp(carryItemTransform.position, machine.carryTransform.position, Time.deltaTime * 5f);
		carryItemTransform.rotation = Quaternion.Lerp(carryItemTransform.rotation , machine.carryTransform.rotation, Time.deltaTime * 5f);
		machine.FollowDestination();
	}
}