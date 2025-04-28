using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class WalkTrigger : MonoBehaviour
{
    [SerializeField] protected UnityEvent onTrigger;
	protected virtual void Action()
	{
		Debug.Log("Trigger", this );
		GetComponent<Collider>().enabled = false;
		if (onTrigger != null)
			onTrigger.Invoke();
		else
			Debug.LogError("onTrigger is null");
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<RaccoonController>(out var o))
			Action();
	}
}
