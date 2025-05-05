using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class ResettableObject : Grabable
{
	public bool needsToBeReset = false;
	public Vector3 originalPos;
	Vector3 orignialEulerAngles;
    protected override void Awake()
    {
		base.Awake();

		navObstacle = GetComponent<NavMeshObstacle>();
        originalPos = transform.position;
		orignialEulerAngles = transform.eulerAngles;
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
	}
	public void ResetPositionAndRotation()
	{
		transform.DOMove(originalPos, 1f);
		transform.DORotate(orignialEulerAngles, 1f);
	}
	void Update()
	{
		float posThreshold = 0.1f;
		float rotThreshold = 5f;

		if (Vector3.Distance(transform.position, originalPos) > posThreshold ||
			Vector3.Angle(transform.eulerAngles, orignialEulerAngles) > rotThreshold)
			needsToBeReset = true;
		else
			needsToBeReset = false;
	}
	NavMeshObstacle navObstacle;
}
