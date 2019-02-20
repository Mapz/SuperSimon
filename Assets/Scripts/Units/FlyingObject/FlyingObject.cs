using UnityEngine;

public abstract class FlyingObject : MovingUnit
{

    

    protected override void OnUpdate()
    {
        DestroyWhenOutOfScreen();
    }

    protected virtual void DestroyWhenOutOfScreen()
    {
        if (CheckOutOfScreen())
        {
            Destroy(gameObject);
        };
    }
}
