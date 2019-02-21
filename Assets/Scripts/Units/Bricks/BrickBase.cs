using DG.Tweening;
using UnityEngine;
public abstract class BrickBase : Unit
{
    public bool m_moveAfterHeadCollision;

    public virtual void OnHeadCollision(Damage dmg)
    {
        GetDamage(dmg);
    }

    protected override void OnAttack(Damage dmg)
    {
        if (m_moveAfterHeadCollision)
        {
            this.transform.DOLocalJump(Vector3.zero, 3, 0, 0.3f).SetRelative();
        }
        base.OnAttack(dmg: dmg);
    }

    public override void OnCollideWithPhysicalObject(RaycastHit2D hit, Collider2D collider)
    {
        
        var colliderMovingUnit = collider.GetComponent<MovingUnit>();
        if (colliderMovingUnit && colliderMovingUnit.m_interactEnviroment)
        {
            if (colliderMovingUnit.m_TrigerBrick)
            {
                if (hit.normal == Vector2.down) // 通过碰撞的法线判断是否从下撞的
                {
                    if (m_moveAfterHeadCollision)
                    {
                        this.transform.DOLocalJump(Vector3.zero, 3, 0, 0.3f).SetRelative();
                    }
                    OnHeadCollision(new Damage(colliderMovingUnit.m_dmg, DmgType.HeadCollision, GetRealDmg));
                }
            }
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
        //TODO: PlayCollisionSound
    //}
}