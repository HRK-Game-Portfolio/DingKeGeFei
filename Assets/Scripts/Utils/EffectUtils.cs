using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUtils : MonoBehaviour {
    private static bool _isUniversalFreeze;
    private static bool _isUniversalSpeedUp;

    public static bool IsUniversalFreeze {
        get => _isUniversalFreeze;
        set => _isUniversalFreeze = value;
    }

    public static bool IsUniversalSpeedUp {
        get => _isUniversalSpeedUp;
        set => _isUniversalSpeedUp = value;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
