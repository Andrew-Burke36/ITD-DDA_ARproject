// Created by Vonce Chew

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource; // Audio Source

    public AudioClip clickAudio; // Click sound audio

    public AudioClip dogBarkAudio; // Dog bark audio

    /// <summary>
    /// Plays click audio when button is clicked
    /// </summary>
    public void PlayClickAudio()
    {
        audioSource.PlayOneShot(clickAudio); // Plays click audio once
    }

    /// <summary>
    /// Plays dog bark audio when button is clicked
    /// </summary>
    public void PlayDogBarkAudio()
    {
        audioSource.PlayOneShot(dogBarkAudio); // Plays dog bark audio once
    }
}
