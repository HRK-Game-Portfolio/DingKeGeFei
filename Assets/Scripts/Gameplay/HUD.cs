using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    private static GameObject _textScoreGameObject;
    private static GameObject _textBallsRemainingGameObject;

    private static Text _textScore;
    private static Text _textBallsRemaining;

    private static int _score = 0;
    private static int _countBallsRemaining;
    private static int _countBlocksRemaining;

    // ======================================================================
    // Properties
    // ======================================================================

    // gets the score  property to be accessed by the game over message prefab
    public static int Score {
        get => _score;
        set => _score = value;
    }

    public static int CountBallsRemaining {
        get => _countBallsRemaining;
        set => _countBallsRemaining = value;
    }

    public static int CountBlocksRemaining {
        get => _countBlocksRemaining;
        set => _countBlocksRemaining = value;
    }

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Start() {
        _textScoreGameObject          = GameObject.FindGameObjectWithTag("TextScore");
        _textBallsRemainingGameObject = GameObject.FindGameObjectWithTag("TextBallsRemaining");

        _textScore          = _textScoreGameObject.GetComponent<Text>();
        _textBallsRemaining = _textBallsRemainingGameObject.GetComponent<Text>();

        // initialise the balls remaining with initial ball counts per game
        _countBallsRemaining = ConfigUtils.BallsPerGame;

        // calculate how many blocks left
        _countBlocksRemaining = FindObjectsOfType<Block>().Length;
        //Debug.Log(_countBlocksRemaining);

        _textScore.text          = "Score: " + _score;
        _textBallsRemaining.text = "Balls: " + _countBallsRemaining;

        EventManager.AddPointsAddedListener(HandlePointsAddedEvent);
        EventManager.AddReduceBallsLeftListener(HandleReduceBallsLeftEvent);
        EventManager.AddLastBallLostListener(HandleGameOverEvent);
        EventManager.AddBlockDestroyedListener(HandleReduceBlocksLeftEvent);
    }

    void Update() { }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    private void HandlePointsAddedEvent(int points) {
        _score += points;
        //Debug.Log(_score);
        if (_textScore != null) {
            _textScore.text = "Score: " + _score;
        }
    }

    private void HandleReduceBallsLeftEvent(int notUsed) {
        CountBallsRemaining--;

        // check if the text object still exist
        // in case the object has been destroyed but other classes are still accessing it
        if (_textBallsRemaining != null) {
            _textBallsRemaining.text = "Balls: " + CountBallsRemaining;
        }
    }

    private void HandleReduceBlocksLeftEvent() {
        CountBlocksRemaining--;
        Debug.Log(CountBlocksRemaining);

        // when last block remaining, adding the listener to game over event
        if (CountBlocksRemaining == 1) {
            EventManager.AddBlockDestroyedListener(HandleGameOverEvent);
        }
    }

    private void HandleGameOverEvent() {
        MenuManager.GoToMenu(MenuName.GameOver);
    }
}