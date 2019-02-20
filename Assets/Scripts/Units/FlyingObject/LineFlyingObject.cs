using UnityEngine;
using System.Collections;

public class LineFlyingObject : FlyingObject
{
    protected override void OnEnabled()
    {
        base.OnEnabled();
        MoveX();
    }
}
