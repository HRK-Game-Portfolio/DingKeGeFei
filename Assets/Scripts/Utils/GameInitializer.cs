using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Initializes the game
public class GameInitializer : MonoBehaviour {
    // Awake is called before Start
    void Awake() {
        // initialize screen utils
        ScreenUtils.Initialize();
        ConfigUtils.Initialize();
    }
}