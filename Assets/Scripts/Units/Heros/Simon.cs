using System.Collections;
using UnityEngine;
public enum SimonStatus
{
    Walk,
    JumpUp0,
    JumpUp1,
    JumpFall0,
    JumpFall1,
    Idle,
    Squat,
    MeleeAttack,
    SquatMeleeAttack,
    OnHit0,
    OnHit1,
    OnHit2,
    Dead,
    SubWeaponAttack,
    Upgrade,

}

public enum ColliderType
{
    Default = 0,
    Squat = 1,
    OnHit = 2,
    None = -1,
}

public class Simon : MovingUnit
{

    public int m_jumpVelocityY = 1000;
    private Animator m_anim;
    public SimonStatus m_status;
    private SimonStatus m_lastStatus;
    private Rigidbody2D m_rigidbody;
    public NonAlwaysActiveWeapons m_whip;
    private float m_lastJumpY;

    public int m_level = 0;

    public int m_maxLevel;

    [SerializeField]
    private NonAlwaysActiveWeapons[] m_whipEachLevel;

    [SerializeField]
    private Collider2D[] colliders;
    /**
        0 -> Stand Collider
        1 -> Squat Collider
     */

    private bool m_wontBeHurt = false;

    public float m_wontBeHurtTime;

    public float m_onHit1Time;

    public float m_hurtJumpVelocityY;

    private Damage m_diedDamage;

    public SubWeaponShooter m_subWeaponShooter;

    public override void Upgrade(int change)
    {
        if (change > 0)
        {
            m_level += change;
            if (m_level >= m_maxLevel)
            {
                m_level = m_maxLevel;
                ChangeState(SimonStatus.Upgrade);
            }
        }
        else if (change < 0)
        {
            m_level += change;
            if (m_level < 0)
            {
                m_level = 0;
                ChangeState(SimonStatus.Upgrade);
            }
        }
        // Upgrade Whip
        m_whip = m_whipEachLevel[m_level];
        m_dmg = m_whip.m_dmg;
    }

    public override void EnableInput()
    {
        GameManager.input.x_axis += Walk;
        GameManager.input.x_axis_add_a += JumpHorizontal;
        GameManager.input.y_axis_down += Squat;
        GameManager.input.no_key += Idle;
        GameManager.input.b += MeleeAttack;
        GameManager.input.y_axis_down_add_b += SquatMeleeAttack;
        GameManager.input.axis_x_add_down += SquatFlip;
        GameManager.input.y_axis_up_and_b += SubWeaponAttack;

    }

    public override void DisableInput()
    {
        GameManager.input.x_axis -= Walk;
        GameManager.input.x_axis_add_a -= JumpHorizontal;
        GameManager.input.y_axis_down -= Squat;
        GameManager.input.no_key -= Idle;
        GameManager.input.b -= MeleeAttack;
        GameManager.input.y_axis_down_add_b -= SquatMeleeAttack;
        GameManager.input.axis_x_add_down -= SquatFlip;
        GameManager.input.y_axis_up_and_b -= SubWeaponAttack;
    }



    //Must Use Custom Mat /Custom/Sprite/Pixel
    void ActiveFlicker(bool active)
    {
        GetComponent<SpriteRenderer>().material.SetInt("_ActiveFlicker", active ? 1 : 0);
    }

    void OnDisable()
    {
        DisableInput();
    }

    void MeleeAttackHitBoxEnable()
    {
        m_whip.ActiveCollider(true);
    }

    void ShootSubWeapon()
    {
        m_subWeaponShooter.ExecShoot();
    }

    IEnumerator AttackOver()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(m_anim.GetCurrentAnimatorStateInfo(0).length);
        m_whip.ActiveCollider(false);
        if (!OnHit())
            ChangeState(SimonStatus.Idle);
    }



    protected override bool CanBeDamaged()
    {
        return !m_wontBeHurt && !m_isDead;
    }

    IEnumerator SquatAttackOver()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(m_anim.GetCurrentAnimatorStateInfo(0).length);
        m_whip.ActiveCollider(false);
        if (!OnHit())
            ChangeState(SimonStatus.Squat);
    }

    IEnumerator SubWeaponAttackOver()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(m_anim.GetCurrentAnimatorStateInfo(0).length);
        if (!OnHit())
            ChangeState(SimonStatus.Idle);
    }

    bool OnHit()
    {
        return (m_status == SimonStatus.OnHit0 || m_status == SimonStatus.OnHit1 || m_status == SimonStatus.OnHit2);
    }

    void UpgradeAnim()
    {
        Time.timeScale = 0;
        DisableInput();
        StartCoroutine(PlayUpgradeAnim());
    }

    IEnumerator PlayUpgradeAnim()
    {
        for (var i = 0; i < 10; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            GetComponent<SpriteRenderer>().material.SetInt("_CurrentPalette", i % 2 == 0 ? 1 : 5);
        }
        GetComponent<SpriteRenderer>().material.SetInt("_CurrentPalette", 1);
        Time.timeScale = 1;
        m_status = m_lastStatus;
        EnableInput();

    }

    void Awake()
    {
        //m_rigidbody = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        ChangeState(SimonStatus.Idle);
        Flip();
    }

    protected override void Die(Damage dmg)
    {
        m_isDead = true;
        m_diedDamage = dmg;
    }

    bool IsColliderActive(ColliderType type)
    {
        if (type == ColliderType.None) return false;
        return colliders[(int)type] == m_currentCollider;
    }

    void ActiveCollider(ColliderType type)
    {
        for (var i = 0; i < colliders.Length; i++)
        {
            if (i == (int)type)
            {
                {
                    if (IsColliderActive(ColliderType.Squat) && (type == ColliderType.Default))
                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 7);
                    if (IsColliderActive(ColliderType.Default) && (type == ColliderType.Squat))
                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 7);
                }
                colliders[i].enabled = true;
                m_currentCollider = colliders[i];
            }
            else
            {
                colliders[i].enabled = false;
            }
        }
    }

    void ChangeState(SimonStatus state)
    {
        if (m_status != state)
        {
            m_lastStatus = m_status;
            m_status = state;
        }
        else
        {
            return;
        }
        Debug.Log("SwitchStatus:" + state);
        switch (state)
        {
            case SimonStatus.JumpUp0:
                m_anim.Play("Idle");
                ActiveCollider(ColliderType.Default);
                break;
            case SimonStatus.JumpUp1:
                ActiveCollider(ColliderType.Squat);
                m_anim.Play("Squat");
                break;
            case SimonStatus.Squat:
                ActiveCollider(ColliderType.Squat);
                m_anim.Play("Squat");
                break;
            case SimonStatus.JumpFall0:
                ActiveCollider(ColliderType.Squat);
                break;
            case SimonStatus.JumpFall1:
                m_anim.Play("Idle");
                ActiveCollider(ColliderType.Default);
                break;
            case SimonStatus.Walk:
                m_anim.Play("Walk");
                ActiveCollider(ColliderType.Default);
                break;
            case SimonStatus.Idle:
                m_anim.Play("Idle");
                if (CheckGrounded())
                {
                    StopX();
                }
                ActiveCollider(ColliderType.Default);
                break;
            case SimonStatus.MeleeAttack:
                //Debug.Log("m_whip.m_animString:" + m_whip.m_animString);
                m_anim.Play(m_whip.m_animString);
                ActiveCollider(ColliderType.Default);
                StartCoroutine(AttackOver());
                break;
            case SimonStatus.SquatMeleeAttack:
                m_anim.Play(m_whip.m_animStringSquat);
                ActiveCollider(ColliderType.Squat);
                StartCoroutine(SquatAttackOver());
                break;
            case SimonStatus.OnHit0:
                m_anim.Play("Onhit");
                ActiveCollider(ColliderType.OnHit);
                m_whip.ActiveCollider(false);
                DisableInput(); //No Input After Attack
                OnDmgAction();
                new EnumTimer(() =>
                {
                    ChangeState(SimonStatus.OnHit1);
                }, 0.1f).StartTimeout(this);
                break;

            case SimonStatus.OnHit1:
                break;

            case SimonStatus.OnHit2:
                m_anim.Play("Squat");
                ActiveCollider(ColliderType.Squat);
                StopX();
                new EnumTimer(() =>
                {
                    if (m_isDead)
                    {
                        ChangeState(SimonStatus.Dead);
                    }
                    else
                    {
                        ChangeState(SimonStatus.Idle);
                        EnableInput();
                    }
                }, m_onHit1Time).StartTimeout(this);
                break;
            case SimonStatus.Dead:
                m_anim.Play("Die");
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 8);
                Destroy(m_rigidbody);
                new EnumTimer(() =>
                {
                    if (OnDied != null)
                    {
                        OnDied(m_diedDamage);
                    }
                }, 2f).StartTimeout(this);
                break;
            case SimonStatus.SubWeaponAttack:
                m_anim.Play("ShootSubWeapon");
                StartCoroutine(SubWeaponAttackOver());
                break;
            case SimonStatus.Upgrade:
                UpgradeAnim();
                break;


        }
    }

    void UpdateStatus()
    {
        if (m_status != SimonStatus.Walk && m_status != SimonStatus.JumpUp0 && m_status != SimonStatus.OnHit0)
        {
            if (CheckGrounded())
            {
                StopX();
            }
        }
        switch (m_status)
        {
            case SimonStatus.Walk:
                //Begin To Fall
                if (physicsObject.velocity.y < 0)
                {

                    StopX();
                    ChangeState(SimonStatus.JumpFall1);
                }
                break;
            case SimonStatus.JumpUp0:
                var JumpHigh = GetJumpHeight();

                if (JumpHigh > 5)
                {
                    ChangeState(SimonStatus.JumpUp1);
                }
                break;
            case SimonStatus.JumpUp1:

                if (CheckGrounded())
                {
                    ChangeState(SimonStatus.Idle);
                }
                break;
            case SimonStatus.JumpFall0:

                JumpHigh = GetJumpHeight();
                if (JumpHigh < 8)
                {
                    ChangeState(SimonStatus.JumpFall1);
                }
                if (CheckGrounded())
                {
                    ChangeState(SimonStatus.Idle);
                }
                break;
            case SimonStatus.JumpFall1:

                if (CheckGrounded())
                {
                    ChangeState(SimonStatus.Idle);
                }
                break;

            case SimonStatus.OnHit1:
                if (CheckGrounded())
                {
                    ChangeState(SimonStatus.OnHit2); // 跪在地上
                }
                break;

        }
    }

    void RestrictPosition()
    {
        if (transform.position.x < InGameVars.LevelConfigs.m_levelMinX)
        {
            transform.position = new Vector2(InGameVars.LevelConfigs.m_levelMinX, transform.position.y);
        }
        if (transform.position.x > InGameVars.LevelConfigs.m_levelMaxX)
        {
            transform.position = new Vector2(InGameVars.LevelConfigs.m_levelMaxX, transform.position.y);
        }
    }

    public void OnTimeOver()
    {
        m_wontBeHurt = false;
        GetDamage(new Damage(m_HP));
    }

    protected override void OnDmg(Damage dmg)
    {
        var dmgXDirection = dmg.xDirection;
        if ((dmgXDirection.x < 0 && !facingRight) || (dmgXDirection.x > 0 && facingRight))
        {
            Flip();
        }
        ChangeState(SimonStatus.OnHit0);
    }

    void OnDmgAction()
    {
        //m_lastJumpY = m_rigidbody.position.y;
        physicsObject.velocity.y = m_hurtJumpVelocityY;
        MoveX(-1);
        m_wontBeHurt = true;
        ActiveFlicker(true);
        new EnumTimer(() =>
        { //无敌时间
            m_wontBeHurt = false;
            ActiveFlicker(false);
        }, m_wontBeHurtTime).StartTimeout(this);
    }

    void FixedUpdate()
    {
        UpdateStatus();
        RestrictPosition();
        CheckDropDie();
    }

    void CheckDropDie()
    {
        if (m_isDead) return;
        if (CheckDropOutScreen())
        {
            m_isDead = true;
            new EnumTimer(() =>
            { //无敌时间
                if (OnDied != null)
                {
                    OnDied(m_diedDamage);
                }
            }, 1.5f).StartTimeout(this);

        }
    }

    float GetJumpHeight()
    {
        return transform.position.y - m_lastJumpY;
    }

    bool CheckIsOnGroundActions()
    {
        return CheckGrounded() && m_status == SimonStatus.Idle || (m_status == SimonStatus.Walk) || (m_status == SimonStatus.Squat);
    }

    bool CheckNotAttacking()
    {
        return m_status != SimonStatus.MeleeAttack && m_status != SimonStatus.SquatMeleeAttack;
    }

    public void SubWeaponAttack()
    {
        Debug.Log("SubWeaponShoot");
        //if (m_status != SimonStatus.Idle) return;
        if (!m_subWeaponShooter.CanShoot) return;
        if (!CheckNotAttacking()) return;
        ChangeState(SimonStatus.SubWeaponAttack);
    }

    public void Idle()
    {
        if (m_status == SimonStatus.Idle) return;
        if (!CheckNotAttacking()) return;
        if (CheckIsOnGroundActions())
        {
            ChangeState(SimonStatus.Idle);
            StopX();
        }
    }

    public void SquatFlip(Vector2 direction)
    {
        if (m_status != SimonStatus.Squat) return;
        if (CheckNotAttacking())
        {
            if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight))
            {
                Flip();
            }
        }
    }

    public void MeleeAttack()
    {
        if (CheckNotAttacking())
        {
            ChangeState(SimonStatus.MeleeAttack);
            if (CheckGrounded())
            {
                StopX();
            }
        }
    }

    public void SquatMeleeAttack()
    {

        if (m_status != SimonStatus.Squat) return;
        if (CheckNotAttacking())
        {
            ChangeState(SimonStatus.SquatMeleeAttack);
            StopX();
        }
    }

    public void Walk(Vector2 direction)
    {
        if (m_status == SimonStatus.Squat)
        {
            ChangeState(SimonStatus.Walk);
            return;
        }
        if (CheckNotAttacking())
            if (CheckIsOnGroundActions())
            {
                ChangeState(SimonStatus.Walk);
                if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight))
                {
                    Flip();
                }
                MoveX();
            }
    }

    public void Squat()
    {
        if (m_status == SimonStatus.Squat) return;
        if (CheckNotAttacking())
            if (CheckIsOnGroundActions())
            {
                ChangeState(SimonStatus.Squat);
                StopX();
            }
    }

    public void JumpHorizontal(int direction)
    {
        Walk(new Vector2(direction, 0));
    }

    public void JumpHorizontal(Vector2 direction)
    {
        if (m_status == SimonStatus.Squat) return;
        if (CheckNotAttacking())
            if (CheckIsOnGroundActions())
            {
                m_lastJumpY = transform.position.y;
                ChangeState(SimonStatus.JumpUp0);
                if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight))
                {
                    Flip();
                }
                if (direction.x != 0)
                {
                    MoveX();
                }
                physicsObject.velocity.y = m_jumpVelocityY;
            }
    }

}