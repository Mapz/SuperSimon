using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** 受击Box by chenhui 2019-4-2 */
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class OnHitBox : MonoBehaviour
{
    private Collider2D m_collider;
    private Rigidbody2D m_rigid;
    private Unit m_UnitAttached;
    public int m_defence;
    public GetRealDmg m_dmgDelegate;
    private bool m_active = true;
    //打击CD计数器，一个武器只能对一个东西周期性造成伤害而不是总是造成伤害
    private OnHitCountDown m_onHitCheck = new OnHitCountDown();

    void Awake()
    {
        // 需要初始化 collider 和 rigid 设置 layer 以及 collider 属性等
        m_UnitAttached = GetComponentInParent<Unit>();
        DamageModifier dm = GetComponent<DamageModifier>();
        if (dm != null){
            m_dmgDelegate = dm.GetRealDmg;
        }
        gameObject.layer = LayerMask.NameToLayer("OnHitBox");
    }

    public void SetActive(bool active)
    {
        m_active = active;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log (name + " . OnTriggerEnter2D (" + other.name + ")");
        _OnTriggerEnter2D(other);
    }

 
    protected virtual void _OnTriggerEnter2D(Collider2D other)
    {
        if (!m_active) return;
        if (m_UnitAttached.m_isDead) return;
        var weapon = other.GetComponent<Weapon>();
        Unit unit;
        if (weapon)
        {
            if (!m_onHitCheck.OnHit(other)) return;
            unit = weapon.m_WeaponCarrier;
            Vector3 hitPos = other.bounds.ClosestPoint(transform.position);
            System.Action OnAttackDelegate = () =>
            {

                Debug.Log("HitPos:" + hitPos);
                m_UnitAttached.OnAttack(new Damage(weapon.m_dmg, weapon.m_dmgType, this.transform.position - other.transform.position, m_dmgDelegate, unit, hitPos));


            };
            switch (m_UnitAttached.m_team)
            {
                case Team.Chaos:
                    switch (weapon.m_team)
                    {
                        case Team.Hero:
                        case Team.Assistance:
                            OnAttackDelegate.Invoke();
                            weapon.OnDealDmg();
                            break;
                    }
                    break;
                case Team.Hero:
                case Team.Assistance:
                    switch (weapon.m_team)
                    {
                        case Team.Enemy:
                        case Team.Enviroment:
                        case Team.Chaos:
                            OnAttackDelegate.Invoke();
                            weapon.OnDealDmg();

                            break;
                    }
                    break;
                case Team.Enemy:
                    switch (weapon.m_team)
                    {
                        case Team.Hero:
                        case Team.Assistance:
                        case Team.Chaos:
                            OnAttackDelegate.Invoke();
                            weapon.OnDealDmg();
                            break;
                    }
                    break;
                case Team.Enviroment:
                    if (weapon.m_destroyEnviroment)
                    {
                        OnAttackDelegate.Invoke();
                        weapon.OnDealDmg();
                    }
                    break;
            }
            //迴旋鏢檢查
            if (weapon is FlyObjectWeapon)
            {
                var flyWeapon = (FlyObjectWeapon)weapon;
                if (flyWeapon.m_Shooter == this && flyWeapon.m_canBeDestroyByShooter)
                {
                    Destroy(flyWeapon.m_WeaponCarrier.gameObject); // TODO:暫時直接毀滅
                }
            }
        }
    }
}
