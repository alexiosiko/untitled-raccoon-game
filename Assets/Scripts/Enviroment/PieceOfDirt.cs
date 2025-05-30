using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PieceOfDirt : MonoBehaviour
{
	bool delay = true;
	void Start() => Invoke(nameof(RemoveDelay), 4f);
	void RemoveDelay() => delay = false;
	void Update()
	{
		if (delay)
			return;

		rb.isKinematic = true;
	}
	void Awake() => rb = GetComponent<Rigidbody>();
	public Rigidbody rb;
}
