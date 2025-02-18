using UnityEngine;
public class RaccoonInteract : MonoBehaviour
{
	float interactRadius = 3f;
    void Update()
    {
		Highlight();
		if (Input.GetKeyDown(KeyCode.E))
			Interact();
    }
	void Interact()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRadius, LayerMask.GetMask("Interactable"));
		if (hitColliders.Length == 0)
			return;

		Interactable i = hitColliders[0].GetComponent<Interactable>();
		i.Action();
	}
	void Highlight()
	{
		// Get all colliders within the interactRadius around the player's position
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRadius, LayerMask.GetMask("Interactable"));

		foreach (Collider collider in hitColliders)
		{
			Interactable interactable = collider.GetComponent<Interactable>();
			if (interactable != null)
			{
				interactable.Highlight();
			}
		}
	}
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, interactRadius);
	}
}
