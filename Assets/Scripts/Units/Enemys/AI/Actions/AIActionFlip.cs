using System.Collections;
using BT;
using UnityEngine;

public class AIActionFlip : BTAction {
    private string _animationName;

    protected override BTResult Execute () {
        Flip ();
        return BTResult.Ended;
    }

    void Flip () {
        MovingUnit unit = database.GetComponent<MovingUnit> ();
        unit.Flip ();
    }
}