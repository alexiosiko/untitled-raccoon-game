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
    [SerializeField] private float maxGrabDistance = 1.5f;
    [SerializeField] private float jointSpring = 500f;
    [SerializeField] private float jointDamper = 10f;
    public RaccoonDraggingState(RaccoonStateMachine machine) : base(RaccoonState.Dragging)
    {
        this.machine = machine;
        mouthSpringJoint = machine.mouthTransform.GetComponent<SpringJoint>();
    }

    public override void EnterState()
    {
		delay = true;
		machine.StartCoroutine(RemoveDelay());
		grabLocalPoint = draggable.transform.InverseTransformPoint(hitPoint);
		SetupGrabJoint(draggable, grabLocalPoint);

    }

    private void SetupGrabJoint(Draggable draggable, Vector3 localGrabPoint)
    {
        mouthSpringJoint.connectedBody = draggable.rb;
        mouthSpringJoint.anchor = Vector3.zero; // Mouth's position
        mouthSpringJoint.connectedAnchor = localGrabPoint;
        mouthSpringJoint.spring = jointSpring;
        mouthSpringJoint.damper = jointDamper;
        mouthSpringJoint.enableCollision = true; // Maintain physics interactions
		mouthSpringJoint.anchor = new (0, 0.25f, 0);
    }

    public override void UpdateState()
    {
        // SpringJoint handles physics automatically
    }

    public override void ExitState()
    {
		mouthSpringJoint.connectedBody = null;
        draggable = null;
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
			Draggable d = hit.collider.GetComponent<Draggable>();
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