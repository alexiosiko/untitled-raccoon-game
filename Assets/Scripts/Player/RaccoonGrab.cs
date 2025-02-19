using DG.Tweening;
using UnityEngine;
public class RaccoonGrab : MonoBehaviour
{
	Animator animator;
	[SerializeField] Transform grabTransform;
	[HideInInspector] public Grabable grabable = null;
	void Grab(Grabable grabable)
	{
		this.grabable = grabable;
		grabable.SetGrabState();
		grabable.transform.SetParent(grabTransform);
		grabable.transform.DOLocalMove(new (0, 0, 0), 0.5f);
		animator.SetBool("Standing", true);
	}
	public void Drop()
	{
		grabable.SetDropState();
		grabable.DOKill();
		grabable.transform.SetParent(GameObject.Find("--- Enviroment ---").transform);
		grabable = null;
		animator.SetBool("Standing", false);

	}
	public void GrabOrDrop(Grabable grabable)
	{
		if (this.grabable == grabable)
			Drop();
		else
			Grab(grabable);
	}
	public static RaccoonGrab Singleton;
	void Awake() 
	{
		Singleton = this;
		animator = GetComponent<Animator>();
	}
}
