using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private GameObject _prefabPaddle;
    [SerializeField] private GameObject _prefabStandardBlock;
    [SerializeField] private GameObject _prefabBonusBlock;
    [SerializeField] private GameObject _prefabPickupBlock;

    // --------------- Fields to be assigned ---------------

    private GameObject _prefabRandomBlock;

    // --------------- Config Params ---------------

    private Vector2 _paddleSpawnPos;

    private float _blockWidth;
    private float _blockHeight;
    private int   _horiBlockCount;
    private float _screenWidth;
    private float _blockGapWidthTotal;
    private float _blockGapWidth;
    private int   _rowCount;
    private float _firstRowYPos;

    // check whether the game has been paused
    private bool _isPaused;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Start() {
        // set game initially to be not paused
        _isPaused = false;

        // spawn paddle at the centre in horizontal direction, vertically slightly above bottom
        _paddleSpawnPos = new Vector2(
            (ScreenUtils.ScreenRight + ScreenUtils.ScreenLeft) / 2,
            ScreenUtils.ScreenBottom + 1);

        Instantiate(_prefabPaddle, _paddleSpawnPos, Quaternion.identity);

        // ------- build the brick row --------
        // the math logic here is basically:
        // - calculate the number of blocks to fulfill one row using Mathf.Floor
        // - using: screen width - number of blocks * block width to figure out total interval width
        //   that has been neglected from the previous step
        // - divide the total interval by (block count - 1) to obtain the width of one single interval
        // - set each block (block width + one single interval) apart from each other
        // - start counting from x = 0, then minus the x-pos of each block by a half screen width
        _screenWidth        = ScreenUtils.ScreenRight * 1;
        _blockWidth         = _prefabStandardBlock.GetComponent<BoxCollider2D>().size.x;
        _horiBlockCount     = (int) Mathf.Floor(_screenWidth / _blockWidth);
        _blockGapWidthTotal = _screenWidth - _blockWidth * _horiBlockCount;
        _blockGapWidth      = _blockGapWidthTotal / (_horiBlockCount - 1);

        _rowCount    = 3;
        _blockHeight = _prefabStandardBlock.GetComponent<BoxCollider2D>().size.y;

        // 1/5 of the screen height down from the top is 3/5 above the horizon
        _firstRowYPos = 3f / 5f * ScreenUtils.ScreenTop;

        BuildBlocks();
    }

    void Update() {
        PauseGame();
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    private void BuildBlocks() {
        List<KeyValuePair<GameObject, float>> blocks = new List<KeyValuePair<GameObject, float>> {
            new KeyValuePair<GameObject, float>(
                _prefabStandardBlock,
                ConfigUtils.StandardBlockProbability),
            new KeyValuePair<GameObject, float>(
                _prefabBonusBlock,
                ConfigUtils.BonusBlockProbability),
            new KeyValuePair<GameObject, float>(
                _prefabPickupBlock,
                ConfigUtils.FreezerBlockProbability + ConfigUtils.SpeedupBlockProbability),
        };

        for (int i = 0; i < _rowCount; i++) {
            for (int j = 0; j < _horiBlockCount; j++) {
                //float randomValue = Random.Range(0, 100);
                //float cumulative  = 0;

                //// We compare that number to each element's cumulative probability and
                //// select the first one that is within range
                //foreach (KeyValuePair<GameObject, float> block in blocks) {
                //    cumulative += block.Value;
                //    if (randomValue < cumulative) {
                //        _prefabRandomBlock = block.Key;
                //        //Debug.Log("The (" + i + ", " + j + ") block; Random Value:" + randomValue 
                //        //    + " is a " + _prefabRandomBlock);

                //        // to break here is very crucial or otherwise the program will always accumulate
                //        // towards the last element show up
                //        break;
                //    }
                //}

                // using reusable separate function from Probability Utility class
                _prefabRandomBlock = Probability.RandomEventsWithProb(blocks, 100);

                Instantiate(_prefabRandomBlock,
                    new Vector2(
                        ScreenUtils.ScreenLeft / 2 + _blockWidth / 2 + j * (_blockWidth + _blockGapWidth),
                        _firstRowYPos - i * _blockHeight),
                    Quaternion.identity);
            }
        }
    }

    private void PauseGame() {
        // pause game on escape key
        // only pause the game when the game has not been paused yet to avoid duplication of pauses
        if (Input.GetKeyDown(KeyCode.Escape) && !MenuManager.IsPaused) {
            MenuManager.GoToMenu(MenuName.Pause);
            MenuManager.IsPaused = true;
        }
    }
}