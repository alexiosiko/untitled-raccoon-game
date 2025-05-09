using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class ResettableObject : Grabable
{
	public bool isGrabbed = false;
	public bool needsToBeReset = false;
	public Vector3 originalPos;
	Vector3 orignialEulerAngles;
	bool delay = true;
    protected override void Awake()
    {
		base.Awake();

		navObstacle = GetComponent<NavMeshObstacle>();
		navObstacle.carving = true;

    }
	void Start()
	{
		// Do htis incase the items needs to fall first
		Invoke(nameof(GetOriginals), 1f);
	}
	void GetOriginals()
	{
		originalPos = transform.position;
		orignialEulerAngles = transform.eulerAngles;
		delay = false;
	}
	public override void SetGrabState(MonoBehaviour sender)
	{
		base.SetGrabState(sender);
		navObstacle.enabled = false;
	}
	public override void SetDropState(MonoBehaviour sender)
	{
		col.enabled = true;
        rb.detectCollisions = true;
		transform.SetParent(GameObject.Find("--- ENVIROMENT ---").transform);
		navObstacle.enabled = true;
		rb.isKinematic = false;
	}
	public void ResetPositionAndRotation()
	{
		transform.DOMove(originalPos, 1f);
		transform.DORotate(orignialEulerAngles, 1f);
	}
	void Update()
	{
		if (delay)
			return;

		float posThreshold = 0.25f;
		float rotThreshold = 10f;

		if (Vector3.Distance(transform.position, originalPos) > posThreshold ||
			Vector3.Angle(transform.eulerAngles, orignialEulerAngles) > rotThreshold)
			needsToBeReset = true;
		else
			needsToBeReset = false;
	}
	NavMeshObstacle navObstacle;
}
