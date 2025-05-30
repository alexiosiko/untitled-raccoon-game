using System;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class EntityFootsteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClips;
    public void PlayFootstepSound()
	{
		float x = animator.GetFloat("Left");
		float y = animator.GetFloat("Forward");
		float speed = Math.Max(Math.Abs(x), Math.Abs(y));
		if  (footstepClips.Length == 0)
		{
			// Debug.LogError("Footstepsclip length is  0");
			return;
		}
		AudioClip clip = footstepClips[UnityEngine.Random.Range(0, footstepClips.Length)];
		float scale = speed;
		if (scale < 0.1f)
			return;
		audioSource.PlayOneShot(clip, scale);
	}
	AudioSource audioSource;
	Animator animator;
    void Awake()
    {
		animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
}
