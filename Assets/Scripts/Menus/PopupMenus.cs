using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Pauses and unpauses the game. Listens for the OnClick events for the pause menu buttons
public class PopupMenus : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    private TextMeshProUGUI _tmpScore;

    // ======================================================================
    // Main Loop & MonoBehaviour Methods
    // ======================================================================

    void Start() {
        // pause the game when added to the scene
        // makes everything stop moving in the game
        Time.timeScale = 0;

        _tmpScore = FindObjectOfType<TextMeshProUGUI>();

        if (_tmpScore != null) {
            _tmpScore.text = "Score: " + HUD.Score;
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // Handles the on click event from the Resume button
    public void HandleResumeButtonOnClickEvent() {
        // unpause game and destroy menu
        Time.timeScale = 1;
        Destroy(gameObject);

        // set the IsPaused property back to false so it could be paused again
        MenuManager.IsPaused = false; 
    }

    // Handles the on click event from the Quit button
    public void HandleQuitButtonOnClickEvent() {
        // unpause game, destroy menu, and go to main menu
        // if we don't unpause the game, then when coming from the main menu next time, the game
        // will remain paused but we want it to start normal instead of paused
        Time.timeScale = 1;
        Destroy(gameObject);

        // set score back to 0 before next session starts
        HUD.Score = 0;
        MenuManager.GoToMenu(MenuName.Main);
    }
}