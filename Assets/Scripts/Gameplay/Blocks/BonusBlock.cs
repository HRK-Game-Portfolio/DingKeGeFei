using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBlock : Block {
    // --------------- Fields to be attached Component Instances ---------------

    private SpriteRenderer _spriteRenderer;

    // ======================================================================
    // Life-Cycle Methods
    // ======================================================================

    protected override void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ScoreWorth = ConfigUtils.BonusBlockPoints;

        base.Start();
    }

    protected override void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Ball")) {
            if (!_isCrashSprite) {
                _spriteRenderer.sprite = CrashSprites[0];

                _isCrashSprite = true;
            }

            _timer.Run();
        }
    }
}