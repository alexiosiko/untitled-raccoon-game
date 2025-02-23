using TMPro;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading.Tasks;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Singleton;
    [SerializeField] TMP_Text text;
    Action onComplete;
	public async void StartNarration(string text, int delayMillisends = 0)
	{
		await Task.Delay(delayMillisends);
		await this.text.DOFade(0, 0.01f).AsyncWaitForCompletion();
		this.text.text = text;
		await this.text.DOFade(1, 1f).AsyncWaitForCompletion();
		await Task.Delay(5000);
		await this.text.DOFade(0, 1f).AsyncWaitForCompletion();
	}
	void Awake() => Singleton = this;
}
