using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages navigation through the menu system, go to the particular menu
public static class MenuManager {
    // check the game has been currently paused or not
    public static bool IsPaused { get; set; } = false;

    // Goes to the menu with the given name
    public static void GoToMenu(MenuName name) {
        switch (name) {
            case MenuName.Main:
                // go to MainMenu scene
                SceneManager.LoadScene("MainMenu");
                break;
            case MenuName.Help:
                // go to HelpMenu scene
                SceneManager.LoadScene("HelpMenu");
                break;
            case MenuName.Pause:
                // instantiate prefab
                // the reason we needed to put that prefab into the `Resources` folder is
                // because the `Resources.Load` method only looks for objects to instantiate
                // in folders called resources and subfolders of folders called `Resources`
                Object.Instantiate(Resources.Load("PauseMenu"));
                break;
            case MenuName.GameOver:
                Object.Instantiate(Resources.Load("GameOverMessage"));
                break;
        }
    }
}