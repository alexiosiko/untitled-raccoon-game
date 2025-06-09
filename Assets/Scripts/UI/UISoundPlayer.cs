using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UISoundPlayer : MonoBehaviour {
    [SerializeField] AudioClip buttonUp, buttonHighlight;
    AudioSource source;
	public void ButtonUp() => source.PlayOneShot(buttonUp);
	public void ButtonHighlight() => source.PlayOneShot(buttonHighlight);
}
