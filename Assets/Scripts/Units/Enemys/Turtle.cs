using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurtleColliderType
{
    Default = 0,
    Guard = 1,
    None = -1,
}
public class Turtle : GuardableEnemy
{
    [SerializeField]
    private Collider2D[] colliders;

    //激活Collider
    void ActiveCollider(TurtleColliderType type)
    {
        //先关闭所有Collider
        for (var i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        Collider2D co = colliders[(int)type];

        co.enabled = true;
        m_currentCollider = co;
    }

    //判断是否当前Collider
    bool IsColliderActive(TurtleColliderType type)
    {
        if (type == TurtleColliderType.None) return false;
        return colliders[(int)type] == m_currentCollider;
    }

    public override void Guard()
    {
        base.Guard();
        ActiveCollider(TurtleColliderType.Guard);
    }

    public override void DisGuard()
    {
        base.DisGuard();
        ActiveCollider(TurtleColliderType.Default);
    }

 
}
