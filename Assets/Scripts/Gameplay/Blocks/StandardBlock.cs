using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBlock : Block {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // --------------- Serialized Cached References ---------------

    [SerializeField] private Sprite[] _standardBlockSpritesArr;

    // --------------- Fields to be attached Component Instances ---------------

    private SpriteRenderer _spriteRenderer;

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    protected override void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        int spriteIndex = Random.Range(0, _standardBlockSpritesArr.Length);
        _spriteRenderer.sprite = _standardBlockSpritesArr[spriteIndex];

        ScoreWorth = ConfigUtils.StandBlockPoints;

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ball")) {
            if (!_isCrashSprite)
            {
                _spriteRenderer.sprite = CrashSprites[0];

                _isCrashSprite = true;
            }

            _timer.Run();
        }
    }
}