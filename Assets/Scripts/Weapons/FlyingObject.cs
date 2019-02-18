using UnityEngine;

public abstract class FlyingObject : MovingUnit
{

    private void FixedUpdate()
    {
        SetPosition();
        DestroyWhenOutOfScreen();
    }

    protected abstract Vector2 SetPosition();

    protected virtual void DestroyWhenOutOfScreen()
    {
        if (CheckOutOfScreen())
        {
            Destroy(gameObject);
        };
    }
}
