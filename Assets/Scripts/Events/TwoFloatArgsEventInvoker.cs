using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Extends MonoBehaviour to support one or more invoking two float arguments UnityEvents
public class TwoFloatArgsEventInvoker : MonoBehaviour {
    protected Dictionary<TwoFloatArgsEventName, UnityEvent<float, float>> UnityEvents =
        new Dictionary<TwoFloatArgsEventName, UnityEvent<float, float>>();

    public void AddTwoFloatArgsListener(
        TwoFloatArgsEventName     twoFloatArgsEventName,
        UnityAction<float, float> listener) {
        if (UnityEvents.ContainsKey(twoFloatArgsEventName)) {
            UnityEvents[twoFloatArgsEventName].AddListener(listener);
        }
    }
}