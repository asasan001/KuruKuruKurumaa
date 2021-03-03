using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{

    AudioSource audioSource;
    [SerializeField] private AudioClip countSound;
    [SerializeField] private AudioClip countFinishSound;
    [SerializeField] private AudioClip gemSound;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip clockSound;
    [SerializeField] private AudioClip carStopSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCountSound() {
        audioSource.PlayOneShot(countSound);
    }
    public void PlayCountFinishSound()
    {
        audioSource.PlayOneShot(countFinishSound);
    }
    public void PlayGemSound()
    {
        audioSource.PlayOneShot(gemSound);
    }
    public void PlayBombSound()
    {
        audioSource.PlayOneShot(bombSound);
    }
    public void PlayClockSound()
    {
        audioSource.PlayOneShot(clockSound);
    }
    public void PlayCarStopSound()
    {
        audioSource.PlayOneShot(carStopSound);
    }
}
