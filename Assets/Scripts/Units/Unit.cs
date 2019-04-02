using BTAI;
using UnityEngine;
using System.Collections.Generic;
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
    public float m_HP
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

    public int m_score; // gain score when died

    public float m_maxHp; 

    public Team m_team;

    public float m_dmg = 0; // save a dmg for other use like create self body weapon or head collision
    private HPBar hpBarAttached;

    [SerializeField]
    private float _HP = 0;

    public bool m_isDead = false;

    public OnDied OnDied;

    private bool m_isPaused = false;

    public bool m_isAttacked { get; private set; }

    public Damage m_lastDamageGot { get; private set; }

    public OnHitBox[] m_onHitBoxes;


    // check if can be damaged
    protected virtual bool CanBeDamaged()
    {
        return !m_wontBeHurt && !m_isDead;
    }

    void Awake()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        m_onHitBoxes = GetComponentsInChildren<OnHitBox>();
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



    protected virtual void OnDmg(Damage dmg)
    {

    }

    protected virtual void OnHeal(float heal)
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
        Debug.Log("Pause");
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

    public virtual void OnAttack(Damage dmg)
    {
        m_isAttacked = true;
        GetDamage(dmg);
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

    //On PhysicalObject Collision
    public virtual void OnCollideWithPhysicalObject(RaycastHit2D hit, Collider2D collider)
    {

    }




}