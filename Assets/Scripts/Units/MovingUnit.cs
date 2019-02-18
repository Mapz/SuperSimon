using UnityEngine;

public abstract class MovingUnit : Unit
{

    public bool facingRight = false;

    public bool m_interactEnviroment;

    /***检查是否在地上 */
    protected bool m_grounded = false;

    [SerializeField]
    public Transform[] m_groundCheck; // 几个位置应当和纵向锚点在一个高度上
    public LayerMask m_groundLayer;

    /***检查是否在地上 end */
    public bool m_TrigerBrick = false; //可以顶砖块

    [System.NonSerialized]
    public Collider2D m_currentCollider;

    public float m_moveSpeed;

    public virtual void Move()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(m_moveSpeed * (facingRight ? 1 : -1), GetComponent<Rigidbody2D>().velocity.y);
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
        if (m_groundCheck != null)
        {
            foreach (var item in m_groundCheck)
            {
                if (Utility.Collision.IsGrounded(this, item, m_groundLayer, 0))
                    return true;
            }
            return false;
        }
        else
        {
            throw new NoGroundCheckObjectException(this.ToString());
        }
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