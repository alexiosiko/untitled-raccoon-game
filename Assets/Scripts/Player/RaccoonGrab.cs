using DG.Tweening;
using UnityEngine;
public class RaccoonGrab : MonoBehaviour
{
	[SerializeField] Transform grabTransform;
	[HideInInspector] public Transform item;
	public void Grab(Transform item)
	{
		this.item = item;
		this.item.SetParent(grabTransform);
	}
	public void Drop()
	{
		item.DOKill();
		item.SetParent(GameObject.Find("--- Enviroment ---").transform);
		item = null;
	}
	void Update()
	{
		if (item == null)
			return;
		item.DOLocalMove(Vector3.zero, 1f);
	}
	public static RaccoonGrab Singleton;
	void Awake() => Singleton = this;
}
