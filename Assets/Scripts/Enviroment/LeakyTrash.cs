using UnityEngine;
using UnityEngine.Events;

public class LeakyTrash : Draggable
{
	[SerializeField] protected UnityEvent onTrigger;
	[SerializeField] int piecesOfTrash = 10;
	[SerializeField] GameObject[] trashPieces;
	Vector3 posOfLastTimeLeakedTrash;
	public override void StartDrag()
	{
		base.StartDrag();
		posOfLastTimeLeakedTrash = transform.position;
	}
	public override void Action(MonoBehaviour caller)
	{
		base.Action(caller);
	}
	protected override void Awake()
	{
		base.Awake();
		posOfLastTimeLeakedTrash = transform.position;
		
	}
	void Update()
	{
		if (piecesOfTrash < 0)
			return;

		if (Vector3.Distance(posOfLastTimeLeakedTrash, transform.position) > 1f)
		{
			piecesOfTrash--;

			posOfLastTimeLeakedTrash = transform.position;
			Instantiate(trashPieces[Random.Range(0, trashPieces.Length)], transform.position, Quaternion.identity, GameObject.Find("--- ENVIROMENT ---").transform);


			if (piecesOfTrash == 0)
				onTrigger?.Invoke();
		}	
	}
}
