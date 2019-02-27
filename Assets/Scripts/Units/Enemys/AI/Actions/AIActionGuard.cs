using System.Collections;
using BT;
using UnityEngine;

public class AIActionGurad : BTAction
{
    private string _animationName;

    protected override BTResult Execute()
    {
        Guard();
        return BTResult.Ended;
    }

    void Guard()
    {
        IGuard guardable = database.GetComponent<IGuard>();
        Debug.Log("Guard");
        guardable.Guard();
    }
}