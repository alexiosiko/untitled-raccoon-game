using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : Interactable
{
	[HideInInspector] public Rigidbody rb; 

	public override void Action(MonoBehaviour caller)
	{

	}
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
}