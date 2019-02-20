using UnityEngine;



[RequireComponent(typeof(PhysicsObject))]
public abstract class MovingUnit : Unit
{
    [SerializeField]
    protected PhysicsObject physicsObject;

    public bool facingRight = false;

    public bool m_interactEnviroment;

    //public LayerMask m_groundLayer;

    public bool m_TrigerBrick = false; //可以顶砖块

    [System.NonSerialized]
    public Collider2D m_currentCollider;

    public float m_moveSpeed;

    int m_moving;

    protected override void OnEnabled()
    {
        physicsObject.ComputeVelocity = UpdateSpeedX;
    }

    void UpdateSpeedX()
    {
        if (m_moving != 0)
            physicsObject.targetVelocity.x = m_moving * m_moveSpeed * (facingRight ? 1 : -1);
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
        return transform.position.y < GameManager.mainCamera.transform.position.y - InGameVars.ScreenHeight / 2 - 20;
    }

    public virtual bool CheckOutOfScreen()
    {
        return transform.position.y < GameManager.mainCamera.transform.position.y - InGameVars.ScreenHeight / 2 - 20 ||
            transform.position.y > GameManager.mainCamera.transform.position.y + InGameVars.ScreenHeight / 2 + 20 ||
            transform.position.x < GameManager.mainCamera.transform.position.x - InGameVars.ScreenWidth / 2 - 20 ||
            transform.position.x > GameManager.mainCamera.transform.position.x + InGameVars.ScreenWidth / 2 + 20;
    }

}