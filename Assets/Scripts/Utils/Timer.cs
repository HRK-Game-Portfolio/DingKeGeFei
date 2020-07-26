using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// A timer
public class Timer : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // timer duration
    private float _totalSeconds = 0;

    // timer execution
    private float _elapsedSeconds = 0;
    private bool  _running        = false;

    // support for Finished property
    private bool _started = false;

    // events invoked by class
    private readonly TimerFinishedEvent _timerFinishedEvent = new TimerFinishedEvent();

    // ======================================================================
    // Properties
    // ======================================================================

    // Sets the duration of the timer
    // The duration can only be set if the timer isn't currently running
    public float Duration {
        set {
            if (!_running) {
                _totalSeconds = value;
            }
        }
    }

    // Gets whether or not the timer has finished running
    // This property returns false if the timer has never been started
    public bool Finished {
        get => _started && !_running;
        set {
            // when finishing the timer, set running to false so `Finished` is true
            _running = !value;
        } 
    }

    // Gets whether or not the timer is currently running
    public bool Running {
        get { return _running; }
    }

    // Gets the total second for duration manipulation
    public float TotalSeconds {
        get => _totalSeconds;
        set => _totalSeconds = value;
    }

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Update() {
        // update timer and check for finished
        if (_running) {
            _elapsedSeconds += Time.deltaTime;
            // elapsed time bigger than total seconds, means longer than the duration
            if (_elapsedSeconds >= _totalSeconds) {
                _running = false;
            }
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // Runs the timer
    // Because a timer of 0 duration doesn't really make sense, the timer only runs
    // if the total seconds is larger than 0. This also makes sure the consumer of the
    // class has actually set the duration to something higher than 0
    public void Run() {
        // only run with valid duration
        if (_totalSeconds > 0) {
            _started        = true;
            _running        = true;
            _elapsedSeconds = 0;
        }
    }

    // Adds the given event handler as a listener
    public void AddTimerFinishedEventListener(UnityAction handler) {
        _timerFinishedEvent.AddListener(handler);
    }
}