using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The audio manager
public static class AudioManager {
    private static bool        _initialized = false;
    private static AudioSource _audioSource;

    private static readonly Dictionary<AudioClipName, AudioClip> _audioClips =
        new Dictionary<AudioClipName, AudioClip>();

    // Gets whether or not the audio manager has been initialized
    public static bool Initialized => _initialized;

    // Initializes the audio manager
    public static void Initialize(AudioSource source) {
        _initialized = true;
        _audioSource = source;
        _audioClips.Add(AudioClipName.LaunchBall,        Resources.Load<AudioClip>("LaunchBall"));
        _audioClips.Add(AudioClipName.BlockCollision,    Resources.Load<AudioClip>("BlockCollision"));
        _audioClips.Add(AudioClipName.LastBallDies,      Resources.Load<AudioClip>("LastBallDies"));
        _audioClips.Add(AudioClipName.LastBlockDestroys, Resources.Load<AudioClip>("LastBlockDestroys"));
    }

    // Plays the audio clip with the given name, when called from other objects
    public static void Play(AudioClipName name) {
        _audioSource.PlayOneShot(_audioClips[name]);
    }
}