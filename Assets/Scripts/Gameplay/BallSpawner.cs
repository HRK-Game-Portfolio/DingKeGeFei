using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject _prefabBall = default;

    [SerializeField] private Text _startText = default;

    // --------------- Fields to be attached Component Instances ---------------

    //private Timer _timer;

    // --------------- Config Params ---------------

    private Vector2 _spawnLocation;
    private bool    _firstBallSpawned;
    private float   _spawnInterval;
    private int     _ballCount;

    // params for checking collider overlapping
    private float _ballColliderHalfWidth;
    private float _ballColliderHalfHeight;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Start() {
        BoxCollider2D collider = _prefabBall.GetComponent<BoxCollider2D>();

        _ballColliderHalfWidth  = collider.size.x / 2;
        _ballColliderHalfHeight = collider.size.y / 2;
        //Debug.Log(_ballColliderHalfWidth);
        //Debug.Log(_ballColliderHalfHeight);

        _firstBallSpawned = false;

        //_timer          = gameObject.AddComponent<Timer>();
        //_timer.Duration = 7.5f;

        EventManager.AddSpawnNewBallsListener(HandleSpawnNewBallEvent);
    }

    void Update() {
        if (!_firstBallSpawned) {
            InitialLaunchOnSpacePressed();
        }

        //if (_timer.Finished) {
        //    HandleSpawnNewBallEvent();
        //    //Debug.Log("Spawning at " + Time.time + "s, due to spawner automatic timer");

        //    _spawnInterval = Random.Range(
        //        ConfigUtils.MinSpawnTime,
        //        ConfigUtils.MaxSpawnTime);
        //    _timer.Duration = _spawnInterval;
        //    _timer.Run();
        //}
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    private void InitialLaunchOnSpacePressed() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            HandleSpawnNewBallEvent();

            // when game starts, get rid of the instruction text
            _startText.text = "";

            //_timer.Run();
        }
    }

    public void HandleSpawnNewBallEvent() {
        if (!_firstBallSpawned) {
            // spawn the first ball at the 1/4 above the bottom of the screen
            _spawnLocation    = new Vector2(0, ScreenUtils.ScreenBottom * 1 / 2);
            _firstBallSpawned = true;
        } else {
            //_spawnLocation = new Vector2(
            //    Random.Range(ScreenUtils.ScreenLeft, ScreenUtils.ScreenRight),
            //    Random.Range(0,                      ScreenUtils.ScreenTop));

            // not so wacky implementation, spawn the ball at some position
            _spawnLocation = new Vector2(0, ScreenUtils.ScreenBottom * 1 / 2);
        }

        // ----- check the box collider overlapping -----
        Vector2 spawnBotLeftVertexPos = new Vector2(
            _spawnLocation.x - _ballColliderHalfWidth,
            _spawnLocation.y - _ballColliderHalfHeight);
        Vector2 spawnTopRightVertexPos = new Vector2(
            _spawnLocation.x + _ballColliderHalfWidth,
            _spawnLocation.y + _ballColliderHalfHeight);

        // Checks if a collider falls within a rectangular area defined by two
        // diagonally opposite corner coordinates in world space
        if (Physics2D.OverlapArea(
                spawnBotLeftVertexPos, spawnTopRightVertexPos) == null) {
            Instantiate(_prefabBall, _spawnLocation, Quaternion.identity);
            _ballCount++;
            //Debug.Log("Spawned " + _ballCount + " balls");
        } else {
            // recurring until spawn at valid position
            HandleSpawnNewBallEvent();
        }
    }
}