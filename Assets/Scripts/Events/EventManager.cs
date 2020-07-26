using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Manages connections between event listeners and event invokers
// so objects can interact with each other without knowing each other
public static class EventManager {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // create lists of invokers because we might have multiple invokers for a particular event
    // the eventName gives us a way to map between invokers and listeners as we're adding invokers
    // and adding listeners through the EventManager

    // --------------- Handle 1 Float Argument --------------- 

    private static readonly
        Dictionary<OneFloatArgEventName, List<OneFloatArgEventInvoker>> OneFloatArgInvokers =
            new Dictionary<OneFloatArgEventName, List<OneFloatArgEventInvoker>>();

    private static readonly
        Dictionary<OneFloatArgEventName, List<UnityAction<float>>> OneFloatArgListeners =
            new Dictionary<OneFloatArgEventName, List<UnityAction<float>>>();

    // --------------- Handle 2 Float Arguments --------------- 

    private static readonly
        Dictionary<TwoFloatArgsEventName, List<TwoFloatArgsEventInvoker>> TwoFloatArgsInvokers =
            new Dictionary<TwoFloatArgsEventName, List<TwoFloatArgsEventInvoker>>();

    private static readonly
        Dictionary<TwoFloatArgsEventName, List<UnityAction<float, float>>> TwoFloatArgsListeners =
            new Dictionary<TwoFloatArgsEventName, List<UnityAction<float, float>>>();


    private static Block                     _pointsAddedEventInvoker;
    private static UnityAction<int>          _pointsAddedEventListener;
    private static PickupBlock               _freezerEventInvoker;
    private static UnityAction<float>        _freezerEventListener;
    private static PickupBlock               _speedUpEventInvoker;
    private static UnityAction<float, float> _speedUpEventListener;
    private static Ball                      _reduceBallsLeftInvoker;
    private static UnityAction<int>          _reduceBallsLeftListener;
    private static Ball                      _spawnNewBallsInvoker;
    private static UnityAction               _spawnNewBallsListener;
    private static Ball                      _lastBallLostInvoker;
    private static UnityAction               _lastBallLostListener;
    private static Block                     _lastBlockDestroyedInvoker;
    private static UnityAction               _lastBlockDestroyedListener;

    public static PickupBlock SpeedUpEventInvoker {
        get => _speedUpEventInvoker;
    }

    public static UnityAction<float, float> SpeedUpEventListener {
        get => _speedUpEventListener;
    }

    // ======================================================================
    // Public Methods
    // ======================================================================

    // ---------------------------------------------------
    // Registering Invokers and Listeners Individually 
    // ---------------------------------------------------

    // --------------- Points Added Events --------------- 

    public static void AddPointsAddedInvoker(Block invoker) {
        _pointsAddedEventInvoker = invoker;
        if (_pointsAddedEventListener != null) {
            _pointsAddedEventInvoker.AddPointsAddedListener(_pointsAddedEventListener);
        }
    }

    public static void AddPointsAddedListener(UnityAction<int> listener) {
        _pointsAddedEventListener = listener;
        if (_pointsAddedEventInvoker != null) {
            _pointsAddedEventInvoker.AddPointsAddedListener(listener);
        }
    }

    // --------------- Freezer Events --------------- 

    public static void AddFreezerInvoker(PickupBlock invoker) {
        _freezerEventInvoker = invoker;
        if (_freezerEventListener != null) {
            // `AddFreezerEffectListener` from PickupBlock class
            _freezerEventInvoker.AddFreezerEffectListener(_freezerEventListener);
        }
    }

    public static void AddFreezerListener(UnityAction<float> listener) {
        _freezerEventListener = listener;
        if (_freezerEventInvoker != null) {
            _freezerEventInvoker.AddFreezerEffectListener(listener);
        }
    }

    // --------------- Speed Up Events --------------- 

    // TODO: make use of the EventManager speed up invoker
    public static void AddSpeedUpInvoker(PickupBlock invoker) {
        _speedUpEventInvoker = invoker;
        if (_speedUpEventListener != null) {
            _speedUpEventInvoker.AddSpeedUpEffectListener(_speedUpEventListener);
            //Debug.Log("Speed Up Invoker added");
        }
    }

    public static void AddSpeedUpListener(UnityAction<float, float> listener) {
        _speedUpEventListener = listener;
        if (_speedUpEventInvoker != null) {
            _speedUpEventInvoker.AddSpeedUpEffectListener(listener);
            //Debug.Log("Speed Up Listener added");
            //Debug.Log(_speedUpEventInvoker);
        }
    }

    // --------------- Reduce Balls Left Events --------------- 

    public static void AddReduceBallsLeftInvoker(Ball invoker) {
        _reduceBallsLeftInvoker = invoker;
        if (_reduceBallsLeftListener != null) {
            _reduceBallsLeftInvoker.AddReduceBallsLeftListener(_reduceBallsLeftListener);
        }
    }

    public static void AddReduceBallsLeftListener(UnityAction<int> listener) {
        _reduceBallsLeftListener = listener;
        if (_reduceBallsLeftInvoker != null) {
            _reduceBallsLeftInvoker.AddReduceBallsLeftListener(listener);
        }
    }

    // --------------- Spawn New Balls Events --------------- 

    public static void AddSpawnNewBallsInvoker(Ball invoker) {
        _spawnNewBallsInvoker = invoker;
        if (_spawnNewBallsListener != null) {
            _spawnNewBallsInvoker.AddSpawningNewBallsListener(_spawnNewBallsListener);
        }
    }

    public static void AddSpawnNewBallsListener(UnityAction listener) {
        _spawnNewBallsListener = listener;
        if (_spawnNewBallsInvoker != null) {
            _spawnNewBallsInvoker.AddSpawningNewBallsListener(listener);
        }
    }

    // --------------- Game Over Events --------------- 

    // Game over event due to losing last ball
    public static void AddLastBallLostInvoker(Ball invoker) {
        _lastBallLostInvoker = invoker;
        if (_lastBallLostListener != null) {
            _lastBallLostInvoker.AddLastBallLostListener(_lastBallLostListener);
        }
    }

    public static void AddLastBallLostListener(UnityAction listener) {
        _lastBallLostListener = listener;
        if (_lastBallLostInvoker != null) {
            _lastBallLostInvoker.AddLastBallLostListener(listener);
        }
    }

    // Game over event due to finishing all the blocks
    public static void AddBlockDestroyedInvoker(Block invoker) {
        _lastBlockDestroyedInvoker = invoker;
        if (_lastBlockDestroyedListener != null) {
            _lastBlockDestroyedInvoker.AddBlockDestroyedListener(_lastBlockDestroyedListener);
        }
    }

    public static void AddBlockDestroyedListener(UnityAction listener) {
        _lastBlockDestroyedListener = listener;
        if (_lastBlockDestroyedInvoker != null) {
            _lastBlockDestroyedInvoker.AddBlockDestroyedListener(listener);
        }
    }

    // ---------------------------------------------------
    // Universal Registering Methods according to Types
    // ---------------------------------------------------

    // --------------- Initialisation --------------- 

    public static void Initialize() {
        // create empty lists for all the dictionary entries
        // foreach goes through each of those five values in EventName enumeration
        foreach (OneFloatArgEventName name in Enum.GetValues(typeof(OneFloatArgEventName))) {
            // if the dictionary doesn't have that name already
            // creates new lists for the invokers and listeners
            if (!OneFloatArgInvokers.ContainsKey(name)) {
                OneFloatArgInvokers.Add(name, new List<OneFloatArgEventInvoker>());
                OneFloatArgListeners.Add(name, new List<UnityAction<float>>());
            } else {
                // if it already has the name, just clear the list
                // we clear the list because the `Initialize` method might be called
                // multiple times as we play the game
                // we don't want to try to add a new list if the dictionary already does contain
                // a particular name, because it throws an exception when trying to add something
                // with the same key as the dictionary already has
                OneFloatArgInvokers[name].Clear();
                OneFloatArgListeners[name].Clear();
            }
        }

        // do exactly the same for the 2 float args event
        foreach (TwoFloatArgsEventName name in Enum.GetValues(typeof(TwoFloatArgsEventName))) {
            if (!TwoFloatArgsInvokers.ContainsKey(name)) {
                TwoFloatArgsInvokers.Add(name, new List<TwoFloatArgsEventInvoker>());
                TwoFloatArgsListeners.Add(name, new List<UnityAction<float, float>>());
            } else {
                TwoFloatArgsInvokers[name].Clear();
                TwoFloatArgsListeners[name].Clear();
            }
        }
    }

    // --------------- Handle 1 Float Argument --------------- 

    #region OneFloatArg

    // Adds the given 1 float arg invoker for the given event name
    public static void AddOneFloatArgInvoker(
        OneFloatArgEventName    eventName,
        OneFloatArgEventInvoker invoker) {
        // add 1 float arg listeners to new invoker and add new invoker to dictionary
        foreach (UnityAction<float> listener in OneFloatArgListeners[eventName]) {
            invoker.AddOneFloatArgListener(eventName, listener);
        }

        OneFloatArgInvokers[eventName].Add(invoker);
    }

    // Adds the given listeners
    public static void AddOneFloatArgListener(
        OneFloatArgEventName eventName,
        UnityAction<float>   listener) {
        // add listener to all invokers and add new listener to dictionary
        foreach (OneFloatArgEventInvoker invoker in OneFloatArgInvokers[eventName]) {
            invoker.AddOneFloatArgListener(eventName, listener);
        }

        OneFloatArgListeners[eventName].Add(listener);
    }

    // Removes the given invoker for the given event name
    // this increase the code efficiency when the invoker has been destroyed
    public static void RemoveOneFloatArgInvoker(
        OneFloatArgEventName    eventName,
        OneFloatArgEventInvoker invoker) {
        // remove invoker from dictionary
        OneFloatArgInvokers[eventName].Remove(invoker);
    }

    #endregion

    // --------------- Handle 2 Float Arguments --------------- 

    #region TwoFloatArgs

    public static void AddTwoFloatArgsInvoker(
        TwoFloatArgsEventName    eventName,
        TwoFloatArgsEventInvoker invoker) {
        foreach (UnityAction<float, float> listener in TwoFloatArgsListeners[eventName]) {
            invoker.AddTwoFloatArgsListener(eventName, listener);
        }

        TwoFloatArgsInvokers[eventName].Add(invoker);
    }

    public static void AddTwoFloatArgsListener(
        TwoFloatArgsEventName     eventName,
        UnityAction<float, float> listener) {
        foreach (TwoFloatArgsEventInvoker invoker in TwoFloatArgsInvokers[eventName]) {
            invoker.AddTwoFloatArgsListener(eventName, listener);
        }

        TwoFloatArgsListeners[eventName].Add(listener);
    }

    public static void RemoveTwoFloatArgInvoker(
        TwoFloatArgsEventName    eventName,
        TwoFloatArgsEventInvoker invoker) {
        TwoFloatArgsInvokers[eventName].Remove(invoker);
    }

    #endregion
}