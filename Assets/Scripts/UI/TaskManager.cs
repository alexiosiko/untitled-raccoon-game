using System;
using UnityEngine;
using UnityEngine.Timeline;

public class TaskManager : MonoBehaviour
{
	[SerializeField] GameObject[] tasks = new GameObject[5];
	[SerializeField] GameObject tasksGameobject;
	public static bool active = true;
	public void SetActive(bool isActive)
	{
		if (isActive)
		{
			tasksGameobject.SetActive(true);
			active = true;

		}
		else
		{
			tasksGameobject.SetActive(false);
			// rect.anchoredPosition = new(0, -1000);
			active = false;
		}
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SetActive(!active);
		}
	}
	RectTransform rect;
	void Awake()
	{
		rect = GetComponent<RectTransform>();
		for (int i = 0; i < tasksGameobject.transform.childCount; i++)
			tasks[i] = tasksGameobject.transform.GetChild(i).gameObject;
			
		SetActive(false);
	}
}