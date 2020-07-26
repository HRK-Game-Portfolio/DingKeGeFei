using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An audio source for the entire game
public class GameAudioSource : MonoBehaviour {
    void Awake() {
        // make sure we only have one of this game object in the game
        // this could happen when going to the menu page and come back
        // a GameAudioSource object will be added to the scene
        // if not Initialized, this is the first game object added to the game
        if (!AudioManager.Initialized) {
            // initialize audio manager and persist audio source across scenes
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioManager.Initialize(audioSource);

            // pass in the game object we want to persist across multiple scenes
            DontDestroyOnLoad(gameObject);
        } else {
            // duplicate game object, so destroy
            Destroy(gameObject);
        }
    }
}