using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : Interactable
{
    [Header("Grab Settings")]
    [SerializeField] private float maxGrabbableMass = 5f;
    [SerializeField] private LayerMask ignoreLayers;
    
    [HideInInspector] public Rigidbody rb;
    private Collider[] colliders;
	
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }
	public override void Action(MonoBehaviour caller)
	{
	}
}