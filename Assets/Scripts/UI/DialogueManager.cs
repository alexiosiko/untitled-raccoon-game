using TMPro;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Singleton;
   	 TMP_Text text;
    Action onComplete;
	public void StartNarration(string text) => StartNarration(text, 0);
	public void StartNarration(string text, int delaySeconds)
	{
		if (narrationCoroutine != null)
		{
			StopCoroutine(narrationCoroutine);
			Debug.Log("Interrupted narrationCoroutine");
		}
		narrationCoroutine = StartCoroutine(NarrationCoroutine(text, delaySeconds));
	}
	Coroutine narrationCoroutine;
	IEnumerator NarrationCoroutine(string text, int delaySeconds = 0)
	{
		yield return new WaitForSeconds(delaySeconds);
		yield return this.text.DOFade(0, 0.01f);
		this.text.text = text;
		yield return this.text.DOFade(1, 1f);
		yield return new WaitForSeconds(5f); // <- fixed
		yield return this.text.DOFade(0, 1f);
	}

	void Awake()
	{
		text = GetComponent<TMP_Text>();
		Singleton = this;
	}
}
