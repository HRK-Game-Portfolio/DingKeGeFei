using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBlock : Block {
    void Start() {
        ScoreWorth = ConfigUtils.BonusBlockPoints;
    }
}