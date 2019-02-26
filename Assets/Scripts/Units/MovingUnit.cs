using UnityEngine;



[RequireComponent(typeof(PhysicsObject))]
public abstract class MovingUnit : Unit
{
    [SerializeField]
    protected PhysicsObject physicsObject;

    public bool facingRight = false; // This Must set as Sprite facing direction does

    public bool m_interactEnviroment;

    public bool m_TrigerBrick = false; //可以顶砖块

    [System.NonSerialized]
    public Collider2D m_currentCollider;

    public float m_xMoveSpeed;

    protected int m_moving;

    protected override void OnEnabled()
    {
        physicsObject.ComputeVelocity = UpdateSpeedX;
    }

    protected virtual void UpdateSpeedX()
    {
        if (m_moving != 0)
            physicsObject.targetVelocity.x = m_moving * m_xMoveSpeed * (facingRight ? 1 : -1);
    }

    public virtual void MoveX(int direction = 1)
    {
        m_moving = direction;
    }

    public virtual void StopX()
    {
        m_moving = 0;
    }

    public virtual void Flip()
    {
        facingRight = !facingRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public virtual bool CheckGrounded()
    {
        return physicsObject.grounded;
    }

    public virtual bool CheckDropOutScreen()
    {
        return Utility.CheckOutOfScreen(transform.position, 20) == Vector2.down;
    }

    public virtual bool CheckOutOfScreen()
    {
        return Utility.CheckOutOfScreen(transform.position, 20) != Vector2.zero;
    }

}