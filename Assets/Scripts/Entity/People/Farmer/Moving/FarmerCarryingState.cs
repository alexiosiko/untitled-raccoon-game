using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FarmerCarryingState : BaseState<FarmerState>
{
	public Transform carryItemTransform;
	private FarmerStateMachine machine;
	public FarmerCarryingState(FarmerStateMachine machine) : base(FarmerState.Carrying) => this.machine = machine;
	bool delay = false;
	IEnumerator UnDelay()
	{
		yield return new WaitForSeconds(1);
		delay = false;
	}
	public override void EnterState()
	{
		delay = true;
		machine.animator.CrossFade("Start Carry", 0.2f);
		machine.agent.isStopped = true;
		machine.StartCoroutine(UnDelay());
		machine.StartCoroutine(AfterPickupLogic());
		carryItemTransform = machine.destinationTransform;
		machine.transform.DOLookAt(carryItemTransform.position, 0.5f, AxisConstraint.Y);
		machine.destinationTransform = null;

		// If carry raccoon
		var s = carryItemTransform.GetComponent<RaccoonStateMachine>();
		if (s)
		{
			s.SetState(RaccoonState.Idle);
			s.GetComponent<Rigidbody>().isKinematic = true;
			machine.destinationTransform = GameObject.Find("Throw Away Transform").transform;
		}


		// If carry resetableObject
		var r = carryItemTransform.GetComponent<ResettableObject>();
		if (r)
		{
			r.SetGrabState(machine);
			Transform target = new GameObject().transform;
			target.position = r.originalPos;
			machine.destinationTransform = target;
		}


	}
	IEnumerator AfterPickupLogic()
	{
		yield return new WaitForSeconds(0.7f);
		
		machine.agent.isStopped = false;
		carryItemTransform.SetParent(machine.carryTransform);
		carryItemTransform.DOLocalMove(Vector3.zero, 1f);
		carryItemTransform.DOLocalRotate(Vector3.zero, 1f);
		var s = carryItemTransform.GetComponent<RaccoonStateMachine>();
		if (s)
		{
			machine.destinationTransform = GameObject.Find("Throw Away Transform").transform;
			yield break;
		}


		// If carry resetableObject
		var r = carryItemTransform.GetComponent<ResettableObject>();
		if (r)
		{
			Transform target = new GameObject().transform;
			target.name = "TEMP DESTINATION";
			target.position = r.originalPos;
			machine.destinationTransform = target;
			yield break;

		}
		
		
	}

	public override IEnumerator ExitState()
	{
		machine.transform.DOLookAt(carryItemTransform.position, 0.5f, AxisConstraint.Y);
		machine.animator.CrossFade("End Carry", 0.2f);
		


		if (machine.destinationTransform.name == "TEMP DESTINATION")
			MonoBehaviour.Destroy(machine.destinationTransform.gameObject);


		machine.destinationTransform = null;
		yield return new WaitForSeconds(0.5f);
		var r = carryItemTransform.GetComponent<ResettableObject>();
		if (r)
		{
			r.SetDropState(machine);
			r.ResetPositionAndRotation();
		}
		carryItemTransform = null;
	}


	

	public override FarmerState GetNextState()
	{
		if (delay || carryItemTransform == null)
			return StateKey;
		
		if (machine.agent.remainingDistance < 1f)
		{
			var s = carryItemTransform.GetComponent<RaccoonStateMachine>();
			if (s) 
			{
				
			}

			var r = carryItemTransform.GetComponent<ResettableObject>();
			if (r)
			{

				return FarmerState.Working;
			}
		}
		return StateKey;
	}

	public override void UpdateState()
	{

		machine.FollowDestination();
	}
}