using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class RaccoonAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    private Animator animator;
    private AudioSource source;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Retrieve movement parameters from the Animator
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        // Calculate the magnitude of movement
        float movementMagnitude = new Vector2(horizontal, vertical).magnitude;

        // Adjust the pitch based on movement magnitude
        // Assuming maximum pitch of 1 at full speed (movementMagnitude of 1)
        source.pitch = Mathf.Lerp(0.2f, 1f, movementMagnitude);

        // Check if the character is moving
        if (movementMagnitude > 0.1f)
        {
            // If the audio is not playing, start playing and fade in
            if (!source.isPlaying)
            {
                Play("grass-footsteps", false);
                source.volume = 0f;
				source.DOKill();
                source.DOFade(1f, 0.1f);
            }
        }
        else
            // If the character stops, fade out and stop the audio
            if (source.isPlaying)
			{
				source.DOKill(	);
				source.DOFade(0f, 0.1f).OnComplete(() => source.Stop());
			}
    }

    private void Play(string name, bool force = true)
    {
        if (!force && source.isPlaying)
            return;

        AudioClip clip = Array.Find(clips, c => c.name == name);
        if (clip == null)
        {
            Debug.LogError("Could not find audio: " + name);
            return;
        }

        source.clip = clip;
        source.loop = true; // Ensure the footstep sound loops
        source.Play();
    }
}
