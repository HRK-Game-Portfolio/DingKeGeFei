using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private Sprite[] _ballSpriteArr;

    // --------------- Fields to be attached Component Instances ---------------

    private Rigidbody2D _rigidbody2D;
    //private Timer       _ballLifeTimer;
    private Timer       _speedUpTimer;

    // the Ball Spawner Script attached to Main Camera
    private BallSpawner _ballSpawner;

    // speed up block invoker - for event handling reference
    private PickupBlock _speedUpBlockInvoker;

    // reduce balls left count & ball dies event - for event handling reference
    private readonly ReduceBallsLeftEvent _reduceBallsLeftEvent = new ReduceBallsLeftEvent();
    private readonly BallDiesEvent        _ballDiesEvent        = new BallDiesEvent();
    private readonly LastBallLostEvent    _lastBallLostEvent    = new LastBallLostEvent();

    // switch sprite timer
    private Timer _spriteTimer;

    private SpriteRenderer _spriteRenderer;

    // --------------- Config Params ---------------

    private float   _launchAngle;
    private Vector2 _launchDir2D;
    private float   _launchDelay;
    private bool    _ballMoving;
    private bool    _isSlowedDown;
    private bool    _isCrashSprite;

    // ======================================================================
    // Main Loop
    // ======================================================================

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _ballSpawner = FindObjectOfType<BallSpawner>();

        _launchAngle   = 20f;
        _launchDir2D   = new Vector2(Mathf.Cos(_launchAngle), Mathf.Sin(_launchAngle));
        _launchDelay   = 1.0f;
        _ballMoving    = false;
        _isSlowedDown  = false;
        _isCrashSprite = false;

        AddTimer();

        //EventManager.AddSpeedUpListener(HandleSpeedUpEvent);

        // directly attaching event listener without using centralised event manager
        // TODO: find a way to implement through a centralised `EventManager`
        _speedUpBlockInvoker = FindObjectOfType<PickupBlock>();
        if (_speedUpBlockInvoker != null) {
            _speedUpBlockInvoker.AddSpeedUpEffectListener(HandleSpeedUpEvent);
        }

        EventManager.AddReduceBallsLeftInvoker(this);
        EventManager.AddSpawnNewBallsInvoker(this);
        EventManager.AddLastBallLostInvoker(this);
    }

    void Update() {
        LaunchBall();
        SpeedDown();
        SwitchBackSprite();
        DestroySelf();
    }

    // using OnBecameInvisible() to implement is tricky because a ball that’s destroying itself
    // due that its death timer expired also becomes invisible as it’s removed from the scene
    // Therefore, finally decided to hard-code in DestroySelf Method
    void OnBecameInvisible() {
        //// remember to add the logic to detect the ball is not disappearing due to timer expired
        //if (!_timer.Finished) {
        //    _ballSpawner.SpawnNewBall();
        //    Destroy(gameObject);
        //}

        // implement ball left calculation in OnBecameInvisible Method 
        // since both disappearing due to timer expire and fallen out of screen count
        // be careful with the text since when closing the game, the text became invisible 
        // but the method is still getting access to it
        //HUD.HandleReduceBallsLeftEvent();
    }

    protected virtual void OnCollisionEnter2D(Collision2D coll) {
        // Debug.Log("------------------------------------");
        // Debug.Log(_speedUpTimer.Finished);
        // Debug.Log(EffectUtils.IsUniversalSpeedUp);
        // Debug.Log(_isSlowedDown);

        if (coll.gameObject.CompareTag("Block") && !_speedUpTimer.Running)
        {
            _spriteTimer.Duration = 0.3f;
            _spriteTimer.Run();

            _spriteRenderer.sprite = _ballSpriteArr[1];

            _isCrashSprite = true;
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    private void LaunchBall() {
        _launchDelay += Time.deltaTime;

        // delay the launch for 3s for the player to know the position of the ball to get ready
        if (_launchDelay >= 3f && !_ballMoving) {
            // determine magnitude of force added according to universal speed up effect
            if (EffectUtils.IsUniversalSpeedUp) {
                _rigidbody2D.AddForce(
                    _launchDir2D * ConfigUtils.BallImpulseForce * ConfigUtils.SpeedUpFactor);
            } else {
                _rigidbody2D.AddForce(_launchDir2D * ConfigUtils.BallImpulseForce);
            }

            _ballMoving = true;

            // play the launch ball audio clip
            AudioManager.Play(AudioClipName.LaunchBall);
        }
    }

    public void SetDirection(Vector2 dir) {
        // direction of the ball based on where it hit the paddle
        _rigidbody2D.velocity = _rigidbody2D.velocity.magnitude * dir;
    }

    private void AddTimer() {
        // set the ball lifetime
        //_ballLifeTimer          = gameObject.AddComponent<Timer>();
        //_ballLifeTimer.Duration = ConfigUtils.BallLifetime;
        //_ballLifeTimer.Run();

        // add the speed up timer for handling later speed up events
        _speedUpTimer = gameObject.AddComponent<Timer>();

        // add the switch sprite timer to handle switching sprite functionality when colliding
        _spriteTimer = gameObject.AddComponent<Timer>();
    }

    private void DestroySelf() {
        //if (_ballLifeTimer.Finished) {
        //    Destroy(gameObject);
        //}

        if (transform.position.y < ScreenUtils.ScreenBottom - 1 ||
            transform.position.y > ScreenUtils.ScreenTop    + 1 ||
            transform.position.x < ScreenUtils.ScreenLeft   - 1 ||
            transform.position.x > ScreenUtils.ScreenRight  + 1) {
            // only reduce the number of balls left when a ball is lost
            _reduceBallsLeftEvent.Invoke(1);
            _ballDiesEvent.Invoke();
            //_ballSpawner.HandleSpawnNewBallEvent();

            // if the ball lost is the last, trigger the game over event
            if (HUD.CountBallsRemaining == 0) {
                _lastBallLostEvent.Invoke();
                AudioManager.Play(AudioClipName.LastBallDies);
            }

            Destroy(gameObject);
        }
    }
    
    private void HandleSpeedUpEvent(float duration, float factor) {
        // !EffectUtils.IsUniversalSpeedUp to make sure no duplication of multiplication
        if (!_speedUpTimer.Running && !EffectUtils.IsUniversalSpeedUp) {
            _rigidbody2D.velocity *= factor;

            _speedUpTimer.Duration = duration;
            _speedUpTimer.Run();
        }

        // Run the universal speed up timer
        SpeedUpEffectMonitor.SwitchOnSpeedUpTimer();
    }

    // TODO: make this a timer finished event
    private void SpeedDown() {
        // use `IsUniversalSpeedUp` property from `EffectUtils` class
        //Debug.Log(EffectUtils.IsUniversalSpeedUp);
        if (_speedUpTimer.Finished && !EffectUtils.IsUniversalSpeedUp && !_isSlowedDown) {
            _rigidbody2D.velocity = new Vector2(
                _rigidbody2D.velocity.x / ConfigUtils.SpeedUpFactor,
                _rigidbody2D.velocity.y / ConfigUtils.SpeedUpFactor);

            // _isSlowedDown field to prevent force being iteratively added
            _isSlowedDown = true;
        }
    }

    // TODO: make this a timer finished event
    private void SwitchBackSprite()
    {
        if (_spriteTimer.Finished && _isCrashSprite)
        {
            _spriteRenderer.sprite = _ballSpriteArr[0];
            _isCrashSprite = false;
        }
    }

    // ----- Register for Reducing Balls Left -----
    public void AddReduceBallsLeftListener(UnityAction<int> listener) {
        _reduceBallsLeftEvent.AddListener(listener);
    }

    // ----- Register for Spawning New Balls -----
    public void AddSpawningNewBallsListener(UnityAction listener) {
        _ballDiesEvent.AddListener(listener);
    }

    // ----- Register for Last Ball Lost -----
    public void AddLastBallLostListener(UnityAction listener) {
        _lastBallLostEvent.AddListener(listener);
    }
}