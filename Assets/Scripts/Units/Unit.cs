using BTAI;
using UnityEngine;

public delegate void OnDied(Damage dmg);

public enum Team
{
    Hero,
    Assistance,
    Enviroment,
    Enemy,
    Chaos,
}

public abstract class Unit : MonoBehaviour, IPause
{
    public int m_HP
    {
        get { return _HP; }
        set
        {
            _HP = value;
            if (hpBarAttached)
            {
                hpBarAttached.RefreshHPBar();
            }
        }
    }

    protected bool m_wontBeHurt = false;

    protected Animator m_anim;

    protected Root m_btAi;

    protected StateMachine m_sm;

    public int m_score;

    public int m_maxHp;

    public Team m_team;

    public int m_dmg = 0;

    public int m_defence = 0;

    private HPBar hpBarAttached;

    [SerializeField]
    private int _HP = 0;

    public bool m_isDead = false;

    public OnDied OnDied;

    private bool m_isPaused = false;

    public bool m_isAttacked { get; private set; }

    public Damage m_lastDamageGot { get; private set; }

    //打击CD计数器，一个武器只能对一个东西周期性造成伤害而不是总是造成伤害
    private OnHitCountDown m_onHitCheck = new OnHitCountDown();


    //是否可以受伤
    protected virtual bool CanBeDamaged()
    {
        return !m_wontBeHurt && !m_isDead;
    }

    public virtual void Upgrade(int change)
    {

    }

    public void AttachToHPBar(HPBar _hp)
    {
        hpBarAttached = _hp;
        hpBarAttached.Attach(this);
    }

    public void DisAttachToHPBar()
    {
        hpBarAttached.DisAttach();
        hpBarAttached = null;
    }


    public void SetFollowByCamera()
    {
        GameManager.cameraHandler.SetFollow(gameObject);
    }

    public virtual void GetDamage(Damage dmg)
    {
        m_lastDamageGot = dmg;

        if (CanBeDamaged())
        {
            if (dmg.rawDamage > 0)
            {
                var realDmg = dmg.realDamage;
                if (realDmg < 0)
                {
                    realDmg = 0;
                }
                m_HP -= realDmg;
                OnDmg(dmg);
                if (m_HP <= 0)
                {
                    Die(dmg);
                }
            }
            else if (dmg.rawDamage < 0)
            {
                m_HP -= dmg.rawDamage;
                OnHeal(-dmg.rawDamage);
                if (m_HP > m_maxHp)
                {
                    m_HP = m_maxHp;
                }
            }
        }
    }

    protected virtual int GetRealDmg(int dmg, DmgType dmgType)
    {
        if (dmgType == DmgType.RealDmg) return dmg;
        return dmg - m_defence;
    }

    protected virtual void OnDmg(Damage dmg)
    {

    }

    protected virtual void OnHeal(int heal)
    {

    }

    protected virtual void OnUpdate()
    {
        if (!m_isDead)
        {
            m_btAi?.Tick();
            m_sm?.OnUpdate();
        }

    }

    protected virtual void OnFixedUpdate()
    {

    }

    protected virtual void OnEnabled()
    {
        AddAI();
    }

    public virtual void EnableInput()
    {

    }

    public virtual void DisableInput()
    {

    }

    public virtual void AddAI()
    {

    }

    public virtual void Pause(bool pause)
    {
        Debug.Log("暂停");
        m_isPaused = pause;
    }

    void OnEnable()
    {
        OnDied += AddScore;
        m_anim = GetComponent<Animator>();
        OnEnabled();
    }

    void Update()
    {
        if (m_isPaused) return;
        OnUpdate();
    }

    void FixedUpdate()
    {
        if (m_isPaused) return;
        m_isAttacked = false;
        OnFixedUpdate();
    }

    void AddScore(Damage dmg)
    {
        if (m_team != Team.Hero && m_team != Team.Assistance)
        {
            InGameVars.score += m_score;
        }
    }


    protected virtual void Die(Damage dmg)
    {
        m_isDead = true;
        OnDied?.Invoke(dmg);
        Destroy(this);

    }

    protected virtual void OnAttack(Damage dmg)
    {
        m_isAttacked = true;
        GetDamage(dmg);
    }


    protected virtual void _OnTriggerEnter2D(Collider2D other)
    {
        if (m_isDead) return;
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
                OnAttack(new Damage(weapon.m_dmg, weapon.m_dmgType, this.transform.position - other.transform.position, GetRealDmg, unit, hitPos));


            };
            switch (m_team)
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

    public static void Destroy(Unit unit)
    {
        ObjectMgr<Unit>.Instance.Destroy(unit);
    }

    public static void Destroy(GameObject go)
    {
        var unit = go.GetComponent<Unit>();
        if (unit)
        {
            ObjectMgr<Unit>.Instance.Destroy(unit);
        }
        else
        {
            Object.Destroy(go);
        }
    }

    //在 PhysicalObject 中触发碰撞
    public virtual void OnCollideWithPhysicalObject(RaycastHit2D hit, Collider2D collider)
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log (name + " . OnTriggerEnter2D (" + other.name + ")");
        _OnTriggerEnter2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log (name + " . OnTriggerExit2D (" + other.name + ")");
    }


}