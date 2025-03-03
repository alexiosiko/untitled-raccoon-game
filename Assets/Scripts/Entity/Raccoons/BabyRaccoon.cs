using System.Threading.Tasks;
using UnityEngine;

public enum BabyRaccoonState {
	FollowingOwner,
	Idle,
	WaitingForFood,
	Eating
}
public class BabyRaccoon : EntityController
{
    public BabyRaccoonState state;
    void Start()
    {
        EnterState(state);
    }
    protected override void Update()
    {
        base.Update();
        UpdateState();
    }
    public override void Action()
    {
        base.Action();
        ActionState();
    }
    public void SetState(BabyRaccoonState newState)
    {
		ExitState();
        state = newState;
        EnterState(state);
    }
    void EnterState(BabyRaccoonState state)
    {
        switch (state)
        {
			case BabyRaccoonState.FollowingOwner:
				destinationTransform = GameObject.Find("Mom").transform;
				break;
            case BabyRaccoonState.WaitingForFood:
				source.clip = Resources.Load<AudioClip>("/Audio/cat-meow.mp3");
				source.PlayDelayed(1);
				destinationTransform = GameObject.Find("Truck").transform;
                DialogueManager.Singleton.StartNarration("Your baby seems to be hungry ... maybe you can find some food from around the corner.", 5000);
				GameObject.Find("Food").GetComponent<Grabable>().interactive = true;
               	InvokeRepeating(nameof(Cry), 7, 12f);
                break;
            case BabyRaccoonState.Eating:
				agent.stoppingDistance = 0.3f;
                destinationTransform = GameObject.Find("Food").transform;
                break;
        }
    }
    void UpdateState()
    {
        switch (state)
        {
            case BabyRaccoonState.WaitingForFood:
                Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
                foreach (Collider c in colliders)
                {
                    if (c.gameObject.name == "Food")
                    {
						c.GetComponent<Grabable>().interactive = false;
                        SetState(BabyRaccoonState.Eating);
                        return;
                    }
                }
                break;
			case BabyRaccoonState.FollowingOwner:
               	Collider[] colliders1 = Physics.OverlapSphere(transform.position, 5f);
                foreach (Collider c in colliders1)
				{
					if (c.name == "Truck")
						SetState(BabyRaccoonState.WaitingForFood);
				}

				break;
			case BabyRaccoonState.Eating:
				if (agent.remainingDistance < 0.5f)
				{
					state = BabyRaccoonState.Idle;
					FinishEating();
				}
				break;
        }
    }
	void ExitState()
	{
        switch (state)
		{
			case BabyRaccoonState.Eating:
				CancelInvoke();
				break;
			case BabyRaccoonState.FollowingOwner:
				destinationTransform = null;
				break;
		}
	}
    void ActionState()
    {

    }

    async void FinishEating()
    {
        animator.Play("Eating");
        await Task.Delay(5000);
    }

    async void Cry()
    {
		await Task.Delay(Random.Range(3, 7) * 1000);

		source.Play();
    }
}
