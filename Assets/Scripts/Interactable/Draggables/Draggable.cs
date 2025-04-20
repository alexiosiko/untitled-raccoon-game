using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : Interactable
{
    [Header("Grab Settings")]
    
    [HideInInspector] public Rigidbody rb;
	
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
	public override void Action(MonoBehaviour caller)
	{
	}
}