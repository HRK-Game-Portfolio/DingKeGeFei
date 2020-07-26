using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Fields to be attached Component Instances ---------------

    private Rigidbody2D   _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    // --------------- Config Params ---------------

    private float _paddleMoveUnitsPerSecond;
    private float _paddleColliderHalfWidth; // stores half collider size for clamping
    private float _screenLeftXPos;
    private float _screenRightXPos;
    private float _paddleTopYPos;

    private const float BounceAngleHalfRange = Mathf.Deg2Rad * 60;

    private Timer _freezeTimer;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Start() {
        _rigidbody2D   = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _paddleMoveUnitsPerSecond = ConfigUtils.PaddleMoveUnitsPerSecond;
        _paddleColliderHalfWidth  = _boxCollider2D.size.x / 2;

        _screenLeftXPos  = ScreenUtils.ScreenLeft  + _paddleColliderHalfWidth;
        _screenRightXPos = ScreenUtils.ScreenRight - _paddleColliderHalfWidth;

        _paddleTopYPos = transform.position.y + _boxCollider2D.size.y / 2;

        _freezeTimer = gameObject.AddComponent<Timer>();
        EventManager.AddFreezerListener(HandleFreezerTimerEvent);
    }

    void Update() {
        // IMPORTANT: remember to put this method in Update instead of FixedUpdate method
        // the physics engine doesn't actually move the rigidbody until after the FixedUpdate
        // method completes so our clamp correction would take place on the frame after the
        // paddle leaves the playfield.
        // if we try the "correct it after it happens" approach, we'll end up with being able to
        // move the paddle partially out of the playfield, especially when the player is holding
        // the movement key down. Creating a trembling effect
        CalculateClampedX();
    }

    void FixedUpdate() {
        HoriMvtHandler();
    }

    // Detects collision with a ball to aim the ball
    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Ball") && CheckCollisionPaddleTop(coll)) {
            // calculate new ball direction
            float ballOffsetFromPaddleCenter = transform.position.x - coll.transform.position.x;

            // calculate the ball position relative to the paddle center
            float normalizedBallOffset = ballOffsetFromPaddleCenter / _paddleColliderHalfWidth;

            // calculate angle offset 
            float angleOffset = normalizedBallOffset * BounceAngleHalfRange;

            float angle = Mathf.PI / 2 + angleOffset;

            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // tell ball to set direction to new direction
            Ball ballScript = coll.gameObject.GetComponent<Ball>();
            ballScript.SetDirection(direction);
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // move the paddle using Rigidbody2D based on users Horizontal input
    private void HoriMvtHandler() {
        float inputHor = 0;

        // assign the horizontal input when the paddle is not in "freeze" state
        if (!_freezeTimer.Running) {
            inputHor = Input.GetAxis("Horizontal");
        }
        
        Vector2 velocity = new Vector2(inputHor * _paddleMoveUnitsPerSecond, 0);

        _rigidbody2D.MovePosition(_rigidbody2D.position + velocity * Time.fixedDeltaTime);
    }

    // clamp the paddle movement to prevent going out of the screen
    private void CalculateClampedX() {
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);

        if (paddlePos.x < _screenLeftXPos || paddlePos.x > _screenRightXPos) {
            // clamp the paddle movement by setting the min and max to the screen boundaries
            paddlePos.x = Mathf.Clamp(paddlePos.x, _screenLeftXPos, _screenRightXPos);

            transform.position = paddlePos;
        }
    }

    // check whether the collision is occuring on the top of the paddle
    private bool CheckCollisionPaddleTop(Collision2D coll) {
        GameObject objHit = coll.gameObject;

        // the y-pos of the bottom of the incoming colliding object during collision
        // is the y-pos of the centre pivot of the object minus half of the object's height
        float halfColliderHeight = objHit.GetComponent<BoxCollider2D>().size.y / 2;
        float colliderBottomYPos = objHit.transform.position.y - halfColliderHeight;

        // 0.05f for tolerance of checking two floats
        return colliderBottomYPos > _paddleTopYPos - 0.05f;
    }

    // --------------- Freeze Event Handler ---------------

    private void HandleFreezerTimerEvent(float duration) {
        _freezeTimer.Duration = duration;
        _freezeTimer.Run();
    }
}