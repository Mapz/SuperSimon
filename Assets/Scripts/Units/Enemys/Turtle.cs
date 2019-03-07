using BTAI;
using DG.Tweening;
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

    [SerializeField]
    private float m_wakeUpTime;

    [SerializeField]
    private float m_wakeUpTime1;

    private float m_currWakeUpTime;

    private bool m_isWakingUp;

    private bool m_isSliding;

    [SerializeField]
    private float m_slidingSpeed;

    private Team m_originalTeam;
    private int m_originalDamage;
    private LayerMask m_originalLayer;

    private Weapon m_selfWeapon;

    protected override void UpdateSpeedX()
    {
        if (m_moving != 0)
            physicsObject.targetVelocity.x = m_moving * (m_isSliding ? m_slidingSpeed : m_xMoveSpeed) * (facingRight ? 1 : -1);
    }

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
        StopX();
        ActiveCollider(TurtleColliderType.Guard);
        m_currWakeUpTime = m_wakeUpTime;
        Debug.Log("Guard");
        if (m_selfWeapon)
        {
            m_selfWeapon.gameObject.SetActive(false);
        }
    }

    public override void DisGuard()
    {
        base.DisGuard();
        ActiveCollider(TurtleColliderType.Default);
        if (m_selfWeapon)
        {
            m_selfWeapon.gameObject.SetActive(true);
        }
    }

    private void Slide()
    {
        m_isSliding = true;
        Debug.Log("Slide");
        m_currWakeUpTime = m_wakeUpTime;
        m_anim.Play("Guard");
        m_originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("OnlyHitGround");
        physicsObject.ResetContactLayer();
        m_interactEnviroment = true;
        if (m_lastDamageGot.dmgDirection.x > 0 != facingRight)
        {
            Flip();
        }
        MoveX();
        if (m_selfWeapon)
        {
            m_originalTeam = m_selfWeapon.m_team;
            m_originalDamage = m_selfWeapon.m_dmg;
            m_selfWeapon.m_team = Team.Chaos;
            m_team = Team.Chaos;
            m_selfWeapon.m_destroyEnviroment = true;
            m_selfWeapon.m_dmg = 10;
            m_selfWeapon.gameObject.SetActive(true);
            m_selfWeapon.m_dmgType = DmgType.RealDmg;
        }
    }

    private void StopSlide()
    {
        m_isSliding = false;
        Debug.Log("StopSlide");
        StopX();
        gameObject.layer = m_originalLayer;
        physicsObject.ResetContactLayer();
        m_interactEnviroment = false;
        if (m_selfWeapon)
        {
            m_selfWeapon.m_team = m_originalTeam;
            m_selfWeapon.m_dmg = m_originalDamage;
            m_team = m_originalTeam;
            m_selfWeapon.m_destroyEnviroment = false;
            m_selfWeapon.gameObject.SetActive(false);
            m_selfWeapon.m_dmgType = DmgType.MeleeWhipPhysics;
        }

    }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        m_selfWeapon = GetComponentInChildren<Weapon>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (m_isSliding) return;
        if (m_isGuarding && m_currWakeUpTime > 0)
        {
            m_currWakeUpTime -= Time.deltaTime;
            if (!m_isWakingUp && m_currWakeUpTime < m_wakeUpTime1)
            {
                m_isWakingUp = true;
                m_anim.Play("WakeUp");
            }
        }
        else if (m_isGuarding && m_currWakeUpTime <= 0)
        {
            DisGuard();
            m_isWakingUp = false;
        }
    }


    protected override void DieFrom(Damage dmg)
    {
        m_anim.Play("Guard");
        base.DieFrom(dmg);
    }


    public override void AddAI()
    {
        base.AddAI();
        m_btAi = BT.Root();
        m_btAi.OpenBranch(
           BT.If(() => { return physicsObject.collided; }).
            OpenBranch(
               BT.Call(Flip),
               BT.Wait(0.2f)
            ),
           BT.If(() => { return m_isAttacked; }).
            OpenBranch(
               BT.Selector().OpenBranch(
                   BT.If(() => { return m_isSliding; }).OpenBranch(
                   BT.Call(
                       StopSlide
                     ),
                   BT.Wait(0.2f)
                   ),
                   BT.If(() => { return m_isGuarding; }).OpenBranch(
                       BT.Call(
                           Slide
                        ),
                       BT.Wait(0.2f)
                       ),

                   BT.Call(Guard),
                   BT.Wait(0.2f)
               )
           ),
           BT.If(() => { return !m_isGuarding; }).
            OpenBranch(
               BT.Call(() => { MoveX(); })
           )

        );
    }


}
