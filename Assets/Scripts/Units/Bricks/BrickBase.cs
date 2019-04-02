using DG.Tweening;
using UnityEngine;
public abstract class BrickBase : Unit
{
    public bool m_moveAfterHeadCollision;

    public virtual void OnHeadCollision(Damage dmg)
    {
        GetDamage(dmg);
    }

    public override void OnAttack(Damage dmg)
    {
        if (m_moveAfterHeadCollision)
        {
            this.transform.DOLocalJump(Vector3.zero, 3, 0, 0.3f).SetRelative();
        }
        base.OnAttack(dmg: dmg);
    }


    //顶砖块后出现一个武器在上方
    private void AddHitCrash(Collider2D collider)
    {
        GameObject hitCrash = new GameObject("HitCrash");
        hitCrash.transform.SetParent(InGameVars.level.transform, true);
        BoxCollider2D thisCollider = GetComponent<BoxCollider2D>();
        hitCrash.transform.position = transform.position + Vector3.up * thisCollider.size.y / 2;
        hitCrash.layer = LayerMask.NameToLayer("Weapons");
        Rigidbody2D rb = hitCrash.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        BoxCollider2D cld = hitCrash.AddComponent<BoxCollider2D>();
        cld.isTrigger = true;
        cld.transform.position = hitCrash.transform.position;
        cld.size = new Vector2(thisCollider.size.x, 3);
        Weapon wp = hitCrash.AddComponent<Weapon>();
        wp.m_collider = cld;
        wp.m_DealDmgToCarrierWhenCollide = false;
        wp.m_destroyEnviroment = false;
        wp.m_dmg = 20;
        wp.m_dmgType = DmgType.RealDmg;
        Unit u = collider.gameObject.GetComponent<Unit>();
        if (u)
        {
            wp.m_team = u.m_team;
        }
        else
        {
            wp.m_team = Team.Enviroment;
        }
        new EnumTimer(() => { Destroy(hitCrash); }, 0.3f).StartTimeout(); //0.3秒后消灭

    }

    public override void OnCollideWithPhysicalObject(RaycastHit2D hit, Collider2D collider)
    {
        if (collider == null) return;
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
                        // 创造一个武器在上方
                        AddHitCrash(collider);

                    }
                    OnHeadCollision(new Damage(colliderMovingUnit.m_dmg, DmgType.HeadCollision, m_onHitBoxes[0].m_dmgDelegate));
                }
            }
        }
    }

}