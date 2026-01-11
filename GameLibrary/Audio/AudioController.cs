using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GameLibrary.Audio;

public class AudioController : IDisposable
{
    private readonly List<SoundEffectInstance> _activeSoundEffectInstances;

    private float _previousSoundEffectVolume;
    private float _previousSoundTrackVolume;

    public AudioController()
    {
        _activeSoundEffectInstances = new List<SoundEffectInstance>();
    }

    public bool IsMuted { get; private set; }
    public bool IsDisposed { get; private set; }

    public float SoundTrackVolume
    {
        get => IsMuted ? 0.0f : MediaPlayer.Volume;

        set
        {
            if (IsMuted) return;

            MediaPlayer.Volume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    public float SoundEffectVolume
    {
        get => IsMuted ? 0.0f : SoundEffect.MasterVolume;

        set
        {
            if (IsMuted) return;

            SoundEffect.MasterVolume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~AudioController()
    {
        Dispose(false);
    }

    protected void Dispose(bool isDisposing)
    {
        if (IsDisposed) return;

        if (isDisposing)
        {
            foreach (var soundEffectInstance in _activeSoundEffectInstances) soundEffectInstance.Dispose();

            _activeSoundEffectInstances.Clear();
        }

        IsDisposed = true;
    }

    public void Update()
    {
        for (var i = _activeSoundEffectInstances.Count - 1; i >= 0; i--)
        {
            var instance = _activeSoundEffectInstances[i];

            if (instance.State == SoundState.Stopped)
            {
                if (!instance.IsDisposed) instance.Dispose();

                _activeSoundEffectInstances.RemoveAt(i);
            }
        }
    }

    public SoundEffectInstance PlaySoundEffect(
        SoundEffect soundEffect,
        float volume = 1.0f,
        float pitch = 0.0f,
        float pan = 0.0f,
        bool isLooped = false)
    {
        var instance = soundEffect.CreateInstance();

        instance.Volume = volume;
        instance.Pitch = pitch;
        instance.Pan = pan;
        instance.IsLooped = isLooped;

        instance.Play();

        _activeSoundEffectInstances.Add(instance);

        return instance;
    }

    public void PlaySoundTrack(Song song, bool isRepeating = true)
    {
        if (MediaPlayer.State == MediaState.Playing) MediaPlayer.Stop();

        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = isRepeating;
    }

    public void PauseAudio()
    {
        MediaPlayer.Pause();

        foreach (var soundEffectInstance in _activeSoundEffectInstances) soundEffectInstance.Pause();
    }

    public void ResumeAudio()
    {
        MediaPlayer.Resume();

        foreach (var soundEffectInstance in _activeSoundEffectInstances) soundEffectInstance.Resume();
    }

    public void MuteAudio()
    {
        _previousSoundEffectVolume = SoundEffect.MasterVolume;
        _previousSoundTrackVolume = MediaPlayer.Volume;

        MediaPlayer.Volume = 0.0f;
        SoundEffect.MasterVolume = 0.0f;

        IsMuted = true;
    }

    public void UnmuteAudio()
    {
        MediaPlayer.Volume = _previousSoundTrackVolume;
        SoundEffect.MasterVolume = _previousSoundEffectVolume;

        IsMuted = false;
    }

    public void ToggleMute()
    {
        if (IsMuted)
            UnmuteAudio();
        else
            MuteAudio();
    }
}
