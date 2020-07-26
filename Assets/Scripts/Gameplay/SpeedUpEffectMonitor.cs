using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEffectMonitor : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // TODO: can later change this to EffectMonitor, not just SpeedUpEffect
    //private Timer _freezerTimer;
    private static Timer _speedUpUniversalTimer;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Start() {
        _speedUpUniversalTimer          = gameObject.AddComponent<Timer>();
        _speedUpUniversalTimer.Duration = ConfigUtils.SpeedUpDuration;

        //Debug.Log(_speedUpUniversalTimer.Running);
    }

    void Update() {
        AssignSpeedUpUtils();

        // just for testing SwitchOnSpeedUpTimer functionality
        if (Input.GetKeyDown(KeyCode.W)) {
            Debug.Log("IsUniversalSpeedUp Before: " + EffectUtils.IsUniversalSpeedUp);
            SwitchOnSpeedUpTimer();
            AssignSpeedUpUtils();
            Debug.Log("IsUniversalSpeedUp After: " + EffectUtils.IsUniversalSpeedUp);
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    public static void SwitchOnSpeedUpTimer() {
        if (!_speedUpUniversalTimer.Running) {
            _speedUpUniversalTimer.Run();
        } else if (_speedUpUniversalTimer.Running) {
            // if the timer is already running, add more running time 
            _speedUpUniversalTimer.TotalSeconds += ConfigUtils.SpeedUpDuration;
        }
    }

    private void AssignSpeedUpUtils() {
        if (_speedUpUniversalTimer.Finished) {
            EffectUtils.IsUniversalSpeedUp = false;
        }

        if (_speedUpUniversalTimer.Running) {
            EffectUtils.IsUniversalSpeedUp = true;
        }
    }
}