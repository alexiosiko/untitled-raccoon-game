using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class Plant : Consumable
{
	public override void Action(MonoBehaviour sender)
	{
		base.Action(sender);
		GetComponent<Collider>().enabled = false;
		sender.SendMessage(nameof(RaccoonStateMachine.SetEatingState), this);
		EatPlant(sender);
	}
	async void EatPlant(MonoBehaviour sender)
	{
		RaccoonStateMachine machine = sender as RaccoonStateMachine;
		transform.SetParent(machine.mouthTransform);
		transform.DOLocalRotate(Vector3.zero, 0.5f);
		transform.DOLocalMove(Vector3.zero, 0.5f);
		await Task.Delay(1000);
		transform.DOScaleY(0, 1f);
		await transform.DOLocalRotate(new Vector3(90, transform.localEulerAngles.x, transform.localEulerAngles.z), 0.5f).AsyncWaitForCompletion();
		await transform.DOScale(Vector3.zero, 1f).AsyncWaitForCompletion();
		sender.SendMessage(nameof(RaccoonStateMachine.SetWalkingState));
		Destroy(gameObject);
	}
}
