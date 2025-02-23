using UnityEditor;
using UnityEngine;
public class RaccoonInteract : MonoBehaviour
{
	float interactRadius = 3f;
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (RaccoonGrab.Singleton.grabable != null)
				RaccoonGrab.Singleton.Drop();
			else
				Interact();
		}
    }
	void Interact()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRadius);
		for (int i = 0; i < hitColliders.Length; i++)
		{
			Interactable interactable = hitColliders[i].GetComponent<Interactable>();
			if (interactable)
			{
				interactable.Action();
				return;
			}
		}
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
				// interactable.Highlight();
			}
		}
	}
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, interactRadius);
	}
}
