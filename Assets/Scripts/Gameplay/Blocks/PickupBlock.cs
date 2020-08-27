using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupBlock : Block {
    // --------------- Serialized Cached References ---------------

    [SerializeField] private Sprite[] _pickupBlockSpriteArr;

    // --------------- Fields to be attached Component Instances ---------------

    private SpriteRenderer _spriteRenderer;

    // freezer event params
    private float                  _freezerDuration;
    private FreezerEffectActivated _freezerEvent;

    // speed up event params
    private       float                  _speedUpDuration;
    private       float                  _speedUpFactor;
    public static SpeedUpEffectActivated SpeedUpEvent;

    private PickupEffect _pickupEffect;

    public PickupEffect PickupEffect {
        get => _pickupEffect;
        set {
            _pickupEffect = value;
            switch (value) {
                case PickupEffect.Freezer:
                    _freezerEvent    = new FreezerEffectActivated();
                    _freezerDuration = ConfigUtils.FreezerDuration;
                    EventManager.AddFreezerInvoker(this);
                    break;
                case PickupEffect.Speedup:
                    SpeedUpEvent     = new SpeedUpEffectActivated();
                    _speedUpDuration = ConfigUtils.SpeedUpDuration;
                    _speedUpFactor   = ConfigUtils.SpeedUpFactor;

                    // directly connect event invoker without using centralised event manager
                    // TODO: find a way to implement through a centralised `EventManager`
                    //EventManager.AddSpeedUpInvoker(this);
                    break;
                default:
                    break;
            }
        }
    }

    // ======================================================================
    // MonoBehaviour Methods
    // ======================================================================

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start() {
        ScoreWorth       = ConfigUtils.PickBlockPoints;
        _freezerDuration = ConfigUtils.FreezerDuration;

        RandomEffect();

        if (PickupEffect == PickupEffect.Freezer) {
            _spriteRenderer.sprite = _pickupBlockSpriteArr[0];
        } else if (PickupEffect == PickupEffect.Speedup) {
            _spriteRenderer.sprite = _pickupBlockSpriteArr[1];
        }

        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    // Invoke the freeze and speed up effect events
    protected override void OnCollisionEnter2D(Collision2D coll) {
        if (PickupEffect == PickupEffect.Freezer) {
            _freezerEvent.Invoke(_freezerDuration);
        }

        if (PickupEffect == PickupEffect.Speedup) {
            //Debug.Log("Speed Up Block Hit");
            SpeedUpEvent.Invoke(_speedUpDuration, _speedUpFactor);

            // VERY IMPORTANT: unsubscribe from the event handler,
            // or the invoker will return null as being destroyed
            // TODO: unsubscribe
        }

        if (coll.gameObject.CompareTag("Ball")) {
            if (!_isCrashSprite) {
                if (PickupEffect == PickupEffect.Freezer) {
                    _spriteRenderer.sprite = CrashSprites[0];
                } else if (PickupEffect == PickupEffect.Speedup) {
                    _spriteRenderer.sprite = CrashSprites[1];
                }

                _isCrashSprite = true;
            }

            _timer.Run();
        }
    }

    // ======================================================================
    // Customised Methods
    // ======================================================================

    // randomly choose which pickup effect to
    private void RandomEffect() {
        List<KeyValuePair<PickupEffect, float>> effects = new List<KeyValuePair<PickupEffect, float>> {
            new KeyValuePair<PickupEffect, float>(
                PickupEffect.Freezer, ConfigUtils.FreezerBlockProbability),
            new KeyValuePair<PickupEffect, float>(
                PickupEffect.Speedup, ConfigUtils.SpeedupBlockProbability)
        };

        //float randomValue = Random.Range(0,
        //    ConfigUtils.FreezerBlockProbability + ConfigUtils.SpeedupBlockProbability);
        //float cumulative = 0;

        // We compare that number to each element's cumulative probability and
        // select the first one that is within range
        //foreach (var effect in effects) {
        //    cumulative += effect.Value;
        //    if (randomValue < cumulative) {
        //        PickupEffect = effect.Key;

        //        // to break here is very crucial or otherwise the program will always accumulate
        //        // towards the last element show up
        //        break;
        //    }
        //}

        // using reusable separate function from Probability Utility class
        PickupEffect = Probability.RandomEventsWithProb(effects,
            ConfigUtils.FreezerBlockProbability + ConfigUtils.SpeedupBlockProbability);

        // alternative way of random value assignment
        // the problem of this implementation is that we can't change the params via config utils
        //int enumLength = System.Enum.GetNames(typeof(PickupEffect)).Length;
        //PickupEffect = (PickupEffect) Random.Range(0, enumLength);
    }

    // let consumers of the class add a listener for the FreezerEffectActivated event
    public void AddFreezerEffectListener(UnityAction<float> listener) {
        _freezerEvent.AddListener(listener);
    }

    // let consumers of the class add a listener for the SpeedUpEffectActivated event
    public void AddSpeedUpEffectListener(UnityAction<float, float> listener) {
        SpeedUpEvent.AddListener(listener);
    }

    // let consumers of the class remove a listener for the SpeedUpEffectActivated event
    // TODO: find a way to implement through a centralised `EventManager`
    public void RemoveSpeedUpEffectListener(UnityAction<float, float> listener) {
        SpeedUpEvent.RemoveListener(listener);
    }
}