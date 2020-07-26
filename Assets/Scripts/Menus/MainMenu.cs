using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Listens for the OnClick events for the main menu buttons
public class MainMenu : MonoBehaviour {
    // Handles the on click event from the play button
    public void HandlePlayButtonOnClickEvent() {
        SceneManager.LoadScene("GamePlay");
    }

    public void HandleHelpButtonOnClickEvent() {
        SceneManager.LoadScene("HelpMenu");
    }

    // Handles the on click event from the quit button
    public void HandleQuitButtonOnClickEvent() {
        Application.Quit();
    }
}