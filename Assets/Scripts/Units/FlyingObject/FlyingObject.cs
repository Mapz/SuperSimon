using UnityEngine;
public class FlyingObject : MovingUnit
{

    public float m_startSpeedY;
    public float m_gravityScale;
    [System.NonSerialized]
    public Unit m_shooter;
    protected PhysicsObject phyo;

    protected override void OnEnabled()
    {
        base.OnEnabled();
        phyo = GetComponent<PhysicsObject>();
        phyo.gravityModifier = m_gravityScale;
        phyo.velocity.y = m_startSpeedY;
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
