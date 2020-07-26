using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Block : MonoBehaviour {
    protected int ScoreWorth;

    private readonly PointsAddedEvent    _pointsAddedEvent    = new PointsAddedEvent();
    private readonly BlockDestroyedEvent _blockDestroyedEvent = new BlockDestroyedEvent();

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    protected virtual void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Ball")) {
            // register for the invokers in EventManager
            EventManager.AddPointsAddedInvoker(this);
            EventManager.AddBlockDestroyedInvoker(this);

            // play the block collision audio
            AudioManager.Play(AudioClipName.BlockCollision);

            // play the last block destroyed audio
            if (HUD.CountBlocksRemaining == 1) {
                AudioManager.Play(AudioClipName.LastBlockDestroys);
            }

            // triggering the event
            _pointsAddedEvent.Invoke(ScoreWorth);
            _blockDestroyedEvent.Invoke();

            Destroy(gameObject);
        }
    }

    // let consumers of the class add a listener for the PointsAddedEvent event
    public void AddPointsAddedListener(UnityAction<int> listener) {
        _pointsAddedEvent.AddListener(listener);
    }

    // let consumers of the class add a listener for the LastBlockDestroyedEvent event
    public void AddBlockDestroyedListener(UnityAction listener) {
        _blockDestroyedEvent.AddListener(listener);
    }
}