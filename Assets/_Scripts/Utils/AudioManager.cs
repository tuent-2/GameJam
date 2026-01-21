using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SceneMonoBehaviour<AudioManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private List<AudioSource> controllableSources;


    public static void PlaySound(AudioClip clip, float volumeScale = 1f)
    {
        if (!clip) return;
        Instance.effectSource.PlayOneShot(clip, volumeScale);
    }

    public static void PlayControllableSource(AudioClip clip, int sourceIndex = 0, float volumeScale = 1f,
        bool isLoop = false)
    {
        Instance._PlayControllableSource(clip, sourceIndex, volumeScale, isLoop);
    }

    // Set clip to null to stop
    private void _PlayControllableSource(AudioClip clip, int sourceIndex = 0, float volumeScale = 1f,
        bool isLoop = false)
    {
        var controllableSource = controllableSources[sourceIndex];
        if (!clip)
        {
            controllableSource.Stop();
            return;
        }

        if (controllableSource.isPlaying) return;
        controllableSource.loop = isLoop;
        controllableSource.clip = clip;
        controllableSource.volume = volumeScale;
        controllableSource.Play();
    }

    public static void StopControllableSource(int sourceIndex = 0)
    {
        Instance._StopControllableSource(sourceIndex);
    }

    // Set clip to null to stop
    private void _StopControllableSource(int sourceIndex = 0)
    {
        controllableSources[sourceIndex].Stop();
    }

    public static void PlayBackgroundMusic(AudioClip clip, float volumeScale = 1f)
    {
        Instance._PlayBackgroundMusic(clip, volumeScale);
    }

    private void _PlayBackgroundMusic(AudioClip clip, float volumeScale = 1f)
    {
        if (!clip)
        {
            musicSource.Stop();
            return;
        }

        musicSource.clip = clip;
        musicSource.volume = volumeScale;
        musicSource.Play();
    }

    public static void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public static void PlayPauseAudio()
    {
        Instance._PlayPauseAudio();
    }

    private void _PlayPauseAudio()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.Play();
        }
    }

    public static void ToggleSoundEffect()
    {
        Instance.effectSource.mute = !Instance.effectSource.mute;
    }

    public static void ToggleMusic()
    {
        Instance.musicSource.mute = !Instance.musicSource.mute;
    }

    public void SetToggleSoundEffect(bool value)
    {
        Instance.effectSource.mute = value;
    }

    public void SetToggleMusic(bool value)
    {
        Instance.musicSource.mute = value;
    }
}