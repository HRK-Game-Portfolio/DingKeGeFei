using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Provides access to configuration data
// This class doesn't inherit from MonoBehaviour because we don't want to attach the class
// to game objects of instantiate it, we just want consumers to access the class directly.
public static class ConfigUtils {
    #region Properties

    private static ConfigData _configData;

    // using expression-body style
    public static float PaddleMoveUnitsPerSecond => _configData.PaddleMoveUnitsPerSecond;
    public static float BallImpulseForce         => _configData.BallImpulseForce;
    public static float BallLifetime             => _configData.BallLifetime;
    public static float MinSpawnTime             => _configData.MinSpawnTime;
    public static float MaxSpawnTime             => _configData.MaxSpawnTime;
    public static int   StandBlockPoints         => _configData.StandardBlockPoints;
    public static int   BonusBlockPoints         => _configData.BonusBlockPoints;
    public static int   PickBlockPoints          => _configData.PickupBlockPoints;
    public static float StandardBlockProbability => _configData.StandardBlockProbability;
    public static float BonusBlockProbability    => _configData.BonusBlockProbability;
    public static float FreezerBlockProbability  => _configData.FreezerBlockProbability;
    public static float SpeedupBlockProbability  => _configData.SpeedupBlockProbability;
    public static int   BallsPerGame             => _configData.BallsPerGame;
    public static float FreezerDuration          => _configData.FreezerDuration;
    public static float SpeedUpDuration          => _configData.SpeedUpDuration;
    public static float SpeedUpFactor            => _configData.SpeedUpFactor;

    #endregion

    // Initializes the configuration utils
    public static void Initialize() {
        _configData = new ConfigData();
    }
}