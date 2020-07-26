using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// A container for the configuration data
public class ConfigData {
    #region Fields

    const string ConfigurationDataFileName = "ConfigurationData.csv";

    // configuration data
    private static float _paddleMoveUnitsPerSecond = 25.0f;
    private static float _ballImpulseForce         = 200.0f;
    private static float _ballLifeTime             = 10.0f;
    private static float _minSpawnTime             = 5.0f;
    private static float _maxSpawnTime             = 10.0f;
    private static int   _standardBlockPoints      = 10;
    private static int   _bonusBlockPoints         = 20;
    private static int   _pickupBlockPoints        = 15;
    private static float _standardBlockProbability = 50.0f;
    private static float _bonusBlockProbability    = 20.0f;
    private static float _freezerBlockProbability  = 15.0f;
    private static float _speedupBlockProbability  = 15.0f;
    private static int   _ballsPerGame             = 30;
    private static float _freezerDuration          = 2.0f;
    private static float _speedUpDuration          = 2.0f;
    private static float _speedUpFactor            = 3.0f;

    #endregion

    #region Properties

    // using expression-body style
    public float PaddleMoveUnitsPerSecond => _paddleMoveUnitsPerSecond;
    public float BallImpulseForce         => _ballImpulseForce;
    public float BallLifetime             => _ballLifeTime;
    public float MinSpawnTime             => _minSpawnTime;
    public float MaxSpawnTime             => _maxSpawnTime;
    public int   StandardBlockPoints      => _standardBlockPoints;
    public int   BonusBlockPoints         => _bonusBlockPoints;
    public int   PickupBlockPoints        => _pickupBlockPoints;
    public float StandardBlockProbability => _standardBlockProbability;
    public float BonusBlockProbability    => _bonusBlockProbability;
    public float FreezerBlockProbability  => _freezerBlockProbability;
    public float SpeedupBlockProbability  => _speedupBlockProbability;
    public int   BallsPerGame             => _ballsPerGame;
    public float FreezerDuration          => _freezerDuration;
    public float SpeedUpDuration          => _speedUpDuration;
    public float SpeedUpFactor            => _speedUpFactor;

    #endregion

    #region Constructor

    // --------------- Constructor ---------------
    // Reads configuration data from a file. If the file read fails,
    // the object contains default values for the configuration data
    public ConfigData() {
        // read and save configuration data from file
        StreamReader input = null;
        try {
            // create stream reader object
            input = File.OpenText(Path.Combine(
                // put the data file into a folder called StreamingAssets
                // so we can use this value to get to that folder location without hard-coding
                Application.streamingAssetsPath, ConfigurationDataFileName));

            // read in names and values
            string names  = input.ReadLine();
            string values = input.ReadLine();

            // set configuration data fields
            SetConfigurationDataFields(values);
        } catch (Exception e) {
            // Remember not to run the program while the csv file is opening
            // The file will be locked and System.IO.IOException error will be returned
            Debug.Log(e);
        } finally {
            // always close input file
            // if close a file that never even opened, will get NullReferenceException
            if (input != null) {
                input.Close();
            }
        }
    }

    // Sets the configuration data fields from the provided csv string
    void SetConfigurationDataFields(string csvValues) {
        string[] valuesSplitArr = csvValues.Split(',');

        _paddleMoveUnitsPerSecond = float.Parse(valuesSplitArr[0]);
        _ballImpulseForce         = float.Parse(valuesSplitArr[1]);
        _ballLifeTime             = float.Parse(valuesSplitArr[2]);
        _minSpawnTime             = float.Parse(valuesSplitArr[3]);
        _maxSpawnTime             = float.Parse(valuesSplitArr[4]);
        _standardBlockPoints      = int.Parse(valuesSplitArr[5]);
        _bonusBlockPoints         = int.Parse(valuesSplitArr[6]);
        _pickupBlockPoints        = int.Parse(valuesSplitArr[7]);
        _standardBlockProbability = float.Parse(valuesSplitArr[8]);
        _bonusBlockProbability    = float.Parse(valuesSplitArr[9]);
        _freezerBlockProbability  = float.Parse(valuesSplitArr[10]);
        _speedupBlockProbability  = float.Parse(valuesSplitArr[11]);
        _ballsPerGame             = int.Parse(valuesSplitArr[12]);
        _freezerDuration          = float.Parse(valuesSplitArr[13]);
        _speedUpDuration          = float.Parse(valuesSplitArr[14]);
        _speedUpFactor            = float.Parse(valuesSplitArr[15]);
    }

    #endregion
}