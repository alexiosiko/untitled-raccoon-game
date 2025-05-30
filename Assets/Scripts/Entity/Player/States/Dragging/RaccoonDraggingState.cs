using System.Collections;
using UnityEngine;

public class RaccoonDraggingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    private static Draggable draggable;
    private SpringJoint mouthSpringJoint;
    private Vector3 grabLocalPoint;
	bool delay = false;
	public static Vector3 hitPoint;
    [Header("Joint Settings")]
    [SerializeField] private float jointSpring = 500f;
    [SerializeField] private float jointDamper = 3f;
    public RaccoonDraggingState(RaccoonStateMachine machine) : base(RaccoonState.Dragging)
    {
        this.machine = machine;
        mouthSpringJoint = machine.mouthTransform.GetComponent<SpringJoint>();
    }

    public override IEnumerator EnterState()
    {
		delay = true;
		machine.StartCoroutine(RemoveDelay());
		draggable.StartDrag();
		grabLocalPoint = draggable.transform.InverseTransformPoint(hitPoint);
		SetupGrabJoint(draggable, grabLocalPoint);
		yield return null;
    }

    private void SetupGrabJoint(Draggable draggable, Vector3 localGrabPoint)
    {
        mouthSpringJoint.connectedBody = draggable.rb;
        mouthSpringJoint.anchor = Vector3.zero; // Mouth's position
        mouthSpringJoint.connectedAnchor = localGrabPoint;
        mouthSpringJoint.spring = jointSpring;
		mouthSpringJoint.minDistance = 0.15f;
		mouthSpringJoint.maxDistance = 0.2f;
        mouthSpringJoint.damper = jointDamper;
        mouthSpringJoint.enableCollision = true; // Maintain physics interactions
		mouthSpringJoint.anchor = new (0, 0.25f, 0);
    }

    public override void UpdateState()
    {
        // SpringJoint handles physics automatically
    }

    public override IEnumerator ExitState()
    {
		mouthSpringJoint.connectedBody = null;
		draggable.StopDrag();
        draggable = null;

		yield return null;
    }

    public override RaccoonState GetNextState()
    {
        if (!delay && Input.GetKeyDown(KeyCode.Space))
            return RaccoonState.Walking;
        return RaccoonState.Dragging;
    }
	public static bool CanDrag(RaccoonStateMachine machine)
	{
		Ray ray = new (machine.controller.centerOfRaccoon, machine.transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, 1f))
		{
			Draggable d = hit.collider.GetComponentInParent<Draggable>();
			if (d)
			{
				draggable = d;
				hitPoint = hit.point;
				d.Action(machine);
				return true;
			}

		}
		return false;
	}
	IEnumerator RemoveDelay()
	{
		yield return new WaitForSeconds(0.25f);
		delay = false;
	}
}