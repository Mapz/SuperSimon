using System.Collections;
using System.Collections.Generic;
using BT = BTAI.BT;
using DG.Tweening;
using UnityEngine;
public class SimpleEnemy : MovingUnit
{
    public bool m_bodyIsWeapon = true; //单位自身是否有判定
    private BoxCollider2D m_weaponCollider;
    private Rigidbody2D m_weaponRigidBody;
    private Weapon m_meBodyWeapon;
    private GameObject m_weaponObject;
    protected bool m_initDirection;


    protected override void OnEnabled()
    {
        base.OnEnabled();
        if (m_bodyIsWeapon)
        {
            m_weaponObject = new GameObject("Weapon");
            m_weaponCollider = m_weaponObject.AddComponent<BoxCollider2D>();
            m_meBodyWeapon = m_weaponObject.AddComponent<Weapon>();
            m_weaponRigidBody = m_weaponObject.AddComponent<Rigidbody2D>();
            m_weaponRigidBody.bodyType = RigidbodyType2D.Kinematic;
            m_meBodyWeapon.m_collider = m_weaponCollider;
            m_weaponCollider.size = GetComponent<BoxCollider2D>().size;
            m_meBodyWeapon.m_destroyEnviroment = m_interactEnviroment;
            m_meBodyWeapon.m_team = m_team;
            m_meBodyWeapon.m_dmg = m_dmg;
            m_weaponCollider.isTrigger = true;
            m_weaponObject.layer = LayerMask.NameToLayer("Weapons");
            m_weaponObject.transform.SetParent(transform, false);
        }
        OnDied += DieFrom;
    }

    protected override void Die(Damage dmg)
    {
        m_isDead = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
        physicsObject.ResetContactLayer();
        Destroy(m_weaponCollider);
        Destroy(m_currentCollider);
        OnDied?.Invoke(dmg);
    }

    protected virtual void DieFrom(Damage dmg)
    {
        if (dmg.dmgType == DmgType.MeleeWhipPhysics || dmg.dmgType == DmgType.RealDmg)
        {
            // 简陋的旋转死亡效果
            var scale = transform.localScale;
            scale.y = -1;
            transform.localScale = scale;
            physicsObject.gravityModifier = 100;
            m_xMoveSpeed = 100;
            physicsObject.velocity.y = 120;
            if (dmg.xDirection.x > 0 != facingRight)
            {
                Flip();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        //掉落坑里，消灭
        if (CheckDropOutScreen())
        {
            Destroy(gameObject);
        }
    }

    public override void AddAI()
    {
        base.AddAI();
        ai = BT.Root();
        ai.OpenBranch(
           BT.If(() => { return m_initDirection == false; }).
            OpenBranch(BT.Call(() =>
            {
                if (facingRight) Flip();
                m_initDirection = true;
            }), BT.Wait(0.2f)),
           BT.If(() => { return physicsObject.collided; }).
            OpenBranch(
               BT.Call(Flip),
               BT.Wait(0.2f)
            ),
           BT.Call(() => { MoveX(); })
        );
    }

}