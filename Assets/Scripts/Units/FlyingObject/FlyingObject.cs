using UnityEngine;
public class FlyingObject : MovingUnit
{

    public float m_startSpeedY;
    public float m_gravityScale;
    [System.NonSerialized]
    public Unit m_shooter;

    protected override void OnEnabled()
    {
        base.OnEnabled();
        physicsObject.gravityModifier = m_gravityScale;
        physicsObject.velocity.y = m_startSpeedY;
        MoveX();
    }

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
