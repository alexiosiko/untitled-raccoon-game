using System.Collections;
using UnityEngine;

public class RaccoonDiggingState : BaseState<RaccoonState>
{
    RaccoonStateMachine machine;
    static Diggable d;

    public RaccoonDiggingState(RaccoonStateMachine machine) : base(RaccoonState.Digging)
    {
        this.machine = machine;
    }

    public override IEnumerator EnterState()
    {
        machine.animator.CrossFade("Digging", 0.2f);

        Vector3 direction = Quaternion.AngleAxis(-12.5f, machine.transform.right) * -machine.transform.forward;
        d.EnterDig(direction);

        yield return new WaitForSeconds(1.85f);
    }

    public override IEnumerator ExitState()
    {
        d.ExitDig();
        yield return null;
    }

    public override RaccoonState GetNextState()
    {
        return RaccoonState.Walking;
    }

    public override void UpdateState() {}

    public static bool CanDig(RaccoonStateMachine machine)
    {
        float maxDistance = 0.2f;
        Vector3 center = machine.controller.centerOfRaccoon;
        Vector3 halfExtends = RaccoonController.halfExtends;
        Quaternion rotation = machine.transform.rotation;

        RaycastHit[] hits = Physics.BoxCastAll(center, halfExtends, Vector3.down, rotation, maxDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out Diggable d))
            {
                RaccoonDiggingState.d = d;
                d.digDustFromPoint = machine.controller.centerOfRaccoon;
                return true;
            }
        }
        return false;
    }
}
