using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Consumable : Interactable
{
    [SerializeField] protected UnityEvent onFinish;
	[SerializeField] int secondsToEat = 2; 
	public override void Action(MonoBehaviour sender)
	{
		GetComponent<Collider>().enabled = false;
		var machine = sender as RaccoonStateMachine;
		var eatingState = machine.States[RaccoonState.Eating] as RaccoonEatingState;
		machine.SetState(RaccoonState.Eating);
		eatingState.consumable = this;
		Eat(sender);


	}
	async void Eat(MonoBehaviour sender)
	{
		RaccoonStateMachine machine = sender as RaccoonStateMachine;
		transform.SetParent(machine.mouthTransform);
		transform.DOLocalRotate(Vector3.zero, 0.5f);
		transform.DOLocalMove(Vector3.zero, 0.5f);
		await Task.Delay(1000);

		transform.DOScaleY(0, 1f);
		await transform.DOLocalRotate(new Vector3(90, transform.localEulerAngles.x, transform.localEulerAngles.z), 0.5f).AsyncWaitForCompletion();
		await Task.Delay (secondsToEat * 1000 - 1000);
		await transform.DOScale(Vector3.zero, 1f).AsyncWaitForCompletion();
		machine.SetState(RaccoonState.Walking);
		onFinish?.Invoke();

		Destroy(gameObject);
	}

}