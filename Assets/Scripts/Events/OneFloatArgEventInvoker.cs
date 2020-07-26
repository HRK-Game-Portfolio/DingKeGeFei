using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Extends MonoBehaviour to support one or more invoking one float argument UnityEvents
public class OneFloatArgEventInvoker : MonoBehaviour {
    // look up values by using keys, the keys don't have to be strings but any data type
    // in this case, keys are enumerations and values int unity events
    // dictionary enables us to invoke more than one event
    protected Dictionary<OneFloatArgEventName, UnityEvent<float>> UnityEvents =
        new Dictionary<OneFloatArgEventName, UnityEvent<float>>();

    // Adds the given listener for the given event name
    public void AddOneFloatArgListener(
        OneFloatArgEventName floatEventName,
        UnityAction<float>   listener) {
        // only add listeners for supported events, `ContainsKey` check for the key
        if (UnityEvents.ContainsKey(floatEventName)) {
            // get the invoker by putting the key in between square brackets
            UnityEvents[floatEventName].AddListener(listener);
        }
    }
}