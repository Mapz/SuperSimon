using System.Collections;
using BT;
using UnityEngine;

public class ActionMove : BTAction {
    private string _animationName;

    protected override BTResult Execute () {
        Move ();
        return BTResult.Ended;
    }

    void Move () {
        MovingUnit unit = database.GetComponent<MovingUnit> ();
        unit.MoveX ();
    }
}