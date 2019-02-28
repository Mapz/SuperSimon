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
        Destroy(m_weaponCollider);
        Destroy(GetComponent<Collider2D>());
        OnDied?.Invoke(dmg);
    }

    protected virtual void DieFrom(Damage dmg)
    {
        if (dmg.dmgType == DmgType.MeleeWhipPhysics|| dmg.dmgType == DmgType.RealDmg)
        {
            // 简陋的旋转死亡效果
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.freezeRotation = false;
            rigid.AddTorque(1000);
            rigid.gravityScale = 100;
            var DeathJumpPointY = GameManager.mainCamera.transform.position.y - InGameVars.ScreenHeight / 2 - transform.position.y;
            var JumpHeight = 3;
            var JumpWidth = 15;
            transform.DOLocalJump(new Vector2(dmg.xDirection.x * JumpWidth, DeathJumpPointY), JumpHeight - DeathJumpPointY, 1, 0.01f * (JumpHeight - DeathJumpPointY)).SetRelative();
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
            OpenBranch(BT.Call(() => { Flip(); m_initDirection = true; }),BT.Wait(0.2f)),
           BT.If(() => { return physicsObject.collided; }).
            OpenBranch(
               BT.Call(Flip),
               BT.Wait(0.2f)
            ),
           BT.Call(() => { MoveX(); })
        );
    }

}