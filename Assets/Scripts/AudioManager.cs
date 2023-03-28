using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource[] AudioTracks;

    private int currentIndex = -1;
    private bool currentlyPlaying = false;

    /// <summary>
    /// Takes in the index of the audio you want to play, then plays it.
    /// </summary>
    /// <param name="index">The index of the audio you want to play</param>
    public void PlayAudio(int index)
    {
        if (index == currentIndex) return;
        if (index < 0 || index >= AudioTracks.Length) return;
        if (currentlyPlaying) AudioTracks[currentIndex].Stop();
        AudioTracks[index].Play();
        currentIndex = index;
        currentlyPlaying = true;
    }

    public void RestartCurrentTrack()
    {
        AudioTracks[currentIndex].Play();
    }

    public void StopAudio()
    {
        AudioTracks[currentIndex].Stop();
        currentIndex = -1;
        currentlyPlaying = false;
    }
}
