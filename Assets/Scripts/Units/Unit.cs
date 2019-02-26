using UnityEngine;

public delegate void OnDied(Damage dmg);

public enum Team
{
    Hero,
    Assistance,
    Enviroment,
    Enemy,
}

public abstract class Unit : MonoBehaviour, PauseAble
{
    public int m_HP
    {
        get { return _HP; }
        set
        {
            _HP = value;
            if (hpBarAttached)
            {
                hpBarAttached.SetHPBar(value);
            }
        }
    }

    public int m_maxHp;

    public Team m_team;

    public int m_dmg = 0;

    public int m_defence = 0;

    private HPBar hpBarAttached;

    [SerializeField]
    private int _HP = 0;

    public bool m_isDead = false;

    public OnDied OnDied;

    //打击CD计数器，一个武器只能对一个东西周期性造成伤害而不是总是造成伤害
    private OnHitCountDown m_onHitCheck = new OnHitCountDown();

    protected virtual bool CanBeDamaged()
    {
        return !m_isDead;
    }

    public virtual void Upgrade(int change)
    {

    }

    public void AttachToHPBar(HPBar _hp)
    {
        hpBarAttached = _hp;
        hpBarAttached.SetHPBar(_HP);
    }

    public void DisAttachToHPBar()
    {
        hpBarAttached.SetHPBar(16);
        hpBarAttached = null;
    }

    public virtual void EnableInput()
    {

    }

    public virtual void DisableInput()
    {

    }

    public void SetFollowByCamera(Camera _camera)
    {
        CameraFollow cameraFollow = gameObject.AddComponent<CameraFollow>();
        cameraFollow.m_camera = _camera;
    }

    public virtual void GetDamage(Damage dmg)
    {
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

    }

    protected virtual void OnFixedUpdate()
    {

    }

    protected virtual void OnEnabled()
    {
    }

    protected virtual void DoPause(bool pause)
    {
    }

    void Pause(bool pause)
    {
        Debug.Log("暂停");
        DoPause(pause);
    }

    void OnEnable()
    {
        OnEnabled();
    }

    void Update()
    {
        OnUpdate();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }


    protected virtual void Die(Damage dmg)
    {
        m_isDead = true;
        OnDied?.Invoke(dmg);
        Destroy(gameObject);

    }

    protected virtual void OnAttack(Damage dmg)
    {
        GetDamage(dmg);
    }

    protected virtual void _OnTriggerEnter2D(Collider2D other)
    {
        if (m_isDead) return;
        var weapon = other.GetComponent<Weapon>();
        if (weapon)
        {
            if (!m_onHitCheck.OnHit(other)) return;
            switch (m_team)
            {
                case Team.Hero:
                case Team.Assistance:
                    switch (weapon.m_team)
                    {
                        case Team.Enemy:
                            OnAttack(new Damage(weapon.m_dmg, weapon.m_dmgType, this.transform.position - other.transform.position, GetRealDmg));
                            weapon.OnDealDmg();
                            break;
                    }
                    break;
                case Team.Enemy:
                    switch (weapon.m_team)
                    {
                        case Team.Hero:
                        case Team.Assistance:
                            OnAttack(new Damage(weapon.m_dmg, weapon.m_dmgType, this.transform.position - other.transform.position, GetRealDmg));
                            weapon.OnDealDmg();
                            break;
                    }
                    break;
                case Team.Enviroment:
                    if (weapon.m_destroyEnviroment)
                    {
                        OnAttack(new Damage(weapon.m_dmg, weapon.m_dmgType, this.transform.position - other.transform.position, GetRealDmg));
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

    void PauseAble.Pause(bool pause)
    {
        throw new System.NotImplementedException();
    }
}