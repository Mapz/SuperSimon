
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

public enum SimonColliderType
{
    Default = 0,
    Squat = 1,
    OnHit = 2,
    None = -1,
}

public class Simon : MovingUnit
{
    private int m_SpriteYOffset1 = 7; // 蹲下和站起
    private int m_SpriteYOffset2 = 6; // 受伤和蹲下
    public int m_jumpVelocityY = 1000;
    public SimonStatus m_status;
    private SimonStatus m_lastStatus;
    private Rigidbody2D m_rigidbody;
    public NonAlwaysActiveWeapons m_whip;

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

    [SerializeField]
    private OnHitBox[] onHitBoxes;
    /**
        0 -> Stand Collider
        1 -> Squat Collider
     */

    public float m_wontBeHurtTime;

    public float m_onHit1Time;

    public float m_hurtJumpVelocityY;

    private Damage m_diedDamage;

    public SubWeaponShooter m_subWeaponShooter;

    /** Unity Called Mehtods**/
    void OnDisable()
    {
        DisableInput();
    }

    protected override void OnUpdate()
    {
        CheckDropDie();
    }

    protected override void OnFixedUpdate()
    {
        UpdateStatus();
        RestrictPosition();
    }

    void Awake()
    {
        m_anim = GetComponent<Animator>();
        ChangeState(SimonStatus.Idle);
        Flip(); //动画是向左的，默认向右
    }

    /** End Of Unity Called Mehtods**/

    /** Override Methods **/
    // 人物升级
    public override void Upgrade(int change)
    {
        if (change > 0)
        {
            m_level += change;
            if (m_level >= m_maxLevel)
            {
                m_level = m_maxLevel;
            }
            ChangeState(SimonStatus.Upgrade);
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


    //开启输入
    public override void EnableInput()
    {
        GameManager.input.x_axis += Walk;
        GameManager.input.x_axis_add_a += Jump;
        GameManager.input.y_axis_down += Squat;
        GameManager.input.no_key += Idle;
        GameManager.input.b += MeleeAttack;
        GameManager.input.y_axis_down_add_b += SquatMeleeAttack;
        GameManager.input.axis_x_add_down += SquatFlip;
        GameManager.input.y_axis_up_and_b += SubWeaponAttack;

    }

    //关闭输入
    public override void DisableInput()
    {
        GameManager.input.x_axis -= Walk;
        GameManager.input.x_axis_add_a -= Jump;
        GameManager.input.y_axis_down -= Squat;
        GameManager.input.no_key -= Idle;
        GameManager.input.b -= MeleeAttack;
        GameManager.input.y_axis_down_add_b -= SquatMeleeAttack;
        GameManager.input.axis_x_add_down -= SquatFlip;
        GameManager.input.y_axis_up_and_b -= SubWeaponAttack;
    }

    protected override void Die(Damage dmg)
    {
        m_isDead = true;
        m_diedDamage = dmg;
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

    /** End of Override Methods **/

    /** Anim Key Frame Functions Do Not Delete **/
    //发射副武器
    void ShootSubWeapon()
    {
        m_subWeaponShooter.ExecShoot();
    }

    // hitbox 开启，动画关键帧调用
    void MeleeAttackHitBoxEnable()
    {
        m_whip.ActiveCollider(true);
        StartCoroutine(DisableMeleeAttackHitBoxByEndOfFrame());
    }
    /** End Of Anim Key Frame Functions Do Not Delete **/

    //开始闪烁
    void ActiveFlicker(bool active)
    {
        GetComponent<SpriteRenderer>().material.SetInt("_ActiveFlicker", active ? 1 : 0);
    }

    // hitbox 关闭，动画关键帧一帧后关闭（鞭子动画持续一帧）
    IEnumerator DisableMeleeAttackHitBoxByEndOfFrame()
    {
        string animName = "Idle";
        switch (m_status)
        {
            case SimonStatus.MeleeAttack:
                animName = m_whip.m_animString;
                break;
            case SimonStatus.SquatMeleeAttack:
                animName = m_whip.m_animStringSquat;
                break;
        }
        //动画的一帧后关闭hitbox
        AnimationClip ac = m_anim.GetAnimationClip(animName);
        float time = 1 / ac.frameRate;
        yield return new WaitForSeconds(time);
        m_whip.ActiveCollider(false);
    }

    //攻击动画不能只用关键帧来结束 ，因为可能在攻击动画未结束的时候切换状态，所以在攻击开始的时候，调用这个协程来关闭攻击动画
    IEnumerator AttackOver(SimonStatus simonStatus)
    {
        string animName = "Idle";
        switch (simonStatus)
        {
            case SimonStatus.MeleeAttack:
                animName = m_whip.m_animString;
                break;
            case SimonStatus.SquatMeleeAttack:
                animName = m_whip.m_animStringSquat;
                break;
            case SimonStatus.SubWeaponAttack:
                animName = "ShootSubWeapon";
                break;
        }
        yield return new WaitForEndOfFrame();
        //获取动画长度
        float time = m_anim.GetAnimationClip(animName).length;
        yield return new WaitForSeconds(time);
        if (!IsOnHit())
        {
            switch (simonStatus)
            {
                case SimonStatus.MeleeAttack:
                    ChangeState(SimonStatus.Idle);
                    break;
                case SimonStatus.SquatMeleeAttack:
                    ChangeState(SimonStatus.Squat);
                    break;
                case SimonStatus.SubWeaponAttack:
                    ChangeState(SimonStatus.Idle);
                    break;
            }
        }

    }

    // 是否正在被打
    bool IsOnHit()
    {
        return (m_status == SimonStatus.OnHit0 || m_status == SimonStatus.OnHit1 || m_status == SimonStatus.OnHit2);
    }

    //升级动画，其他的暂停
    void UpgradeAnim()
    {
        Time.timeScale = 0;
        DisableInput();
        StartCoroutine(PlayUpgradeAnim());
    }

    //升级动画
    IEnumerator PlayUpgradeAnim()
    {
        for (var i = 0; i < 10; i++)
        {
            //TimeScalse == 0 的时候，使用realtime来做协程
            yield return new WaitForSecondsRealtime(0.1f);
            //使用调色板来做动画
            GetComponent<SpriteRenderer>().material.SetInt("_CurrentPalette", i % 2 == 0 ? 1 : 5);
        }
        GetComponent<SpriteRenderer>().material.SetInt("_CurrentPalette", 1);
        Time.timeScale = 1;
        m_status = m_lastStatus;
        EnableInput();
    }

    //判断是否当前Collider
    bool IsColliderActive(SimonColliderType type)
    {
        if (type == SimonColliderType.None) return false;
        return colliders[(int)type] == m_currentCollider;
    }

    //激活Collider
    void ActiveCollider(SimonColliderType type)
    {
        //先关闭所有Collider
        for (var i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
            onHitBoxes[i].gameObject.SetActive(false);
        }

        Collider2D co = colliders[(int)type];

        //不同的Collider需要改变人物的位置
        int isGrounded = CheckGrounded() ? 1 : 0;
        if (IsColliderActive(SimonColliderType.Squat) && (type == SimonColliderType.Default))
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + isGrounded * m_SpriteYOffset1);
        else if (IsColliderActive(SimonColliderType.Default) && (type == SimonColliderType.Squat))
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - isGrounded * m_SpriteYOffset1);
        else if (IsColliderActive(SimonColliderType.OnHit) && (type == SimonColliderType.Squat))
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - m_SpriteYOffset2);
        else if (IsColliderActive(SimonColliderType.Squat) && (type == SimonColliderType.OnHit))
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + m_SpriteYOffset2);


        co.enabled = true;
        onHitBoxes[(int)type].gameObject.SetActive(true);
        m_currentCollider = co;


    }

    //切换状态
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
        //Debug.Log("SwitchStatus:" + state);
        switch (state)
        {
            case SimonStatus.JumpUp0:
                m_anim.Play("Idle");
                ActiveCollider(SimonColliderType.Default);
                break;
            case SimonStatus.JumpUp1:
                ActiveCollider(SimonColliderType.Squat);
                m_anim.Play("Squat");
                break;
            case SimonStatus.Squat:
                ActiveCollider(SimonColliderType.Squat);
                m_anim.Play("Squat");
                break;
            case SimonStatus.JumpFall0:
                ActiveCollider(SimonColliderType.Squat);
                break;
            case SimonStatus.JumpFall1:
                m_anim.Play("Idle");
                ActiveCollider(SimonColliderType.Default);
                break;
            case SimonStatus.Walk:
                m_anim.Play("Walk");
                ActiveCollider(SimonColliderType.Default);
                break;
            case SimonStatus.Idle:
                m_anim.Play("Idle");
                if (CheckGrounded())
                {
                    StopX();
                }
                ActiveCollider(SimonColliderType.Default);
                break;
            case SimonStatus.MeleeAttack:
                m_anim.Play(m_whip.m_animString);
                ActiveCollider(SimonColliderType.Default);
                StartCoroutine(AttackOver(state));
                break;
            case SimonStatus.SquatMeleeAttack:
                m_anim.Play(m_whip.m_animStringSquat);
                ActiveCollider(SimonColliderType.Squat);
                StartCoroutine(AttackOver(state));
                break;
            case SimonStatus.OnHit0:
                m_anim.Play("Onhit");
                ActiveCollider(SimonColliderType.OnHit);
                m_whip.ActiveCollider(false);
                DisableInput(); //No Input After OnHit
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
                ActiveCollider(SimonColliderType.Squat);
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
                    OnDied?.Invoke(m_diedDamage);
                }, 2f).StartTimeout(this);
                break;
            case SimonStatus.SubWeaponAttack:
                m_anim.Play("ShootSubWeapon");
                ActiveCollider(SimonColliderType.Default);
                StartCoroutine(AttackOver(state));
                break;
            case SimonStatus.Upgrade:
                UpgradeAnim();
                break;
        }
    }


    //状态机Update
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
                //开始掉落
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
                JumpHigh = GetJumpHeight();
                if (CheckGrounded())
                {
                    ChangeState(SimonStatus.Idle);
                }
                break;
            case SimonStatus.JumpFall0:

                JumpHigh = GetJumpHeight();
                if (JumpHigh < m_SpriteYOffset1 + 1)
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

    // 限制移动区域 TODO：要和物理引擎结合一下
    void RestrictPosition()
    {
        if (transform.position.x < InGameVars.LevelConfigs.m_levelMinX)
        {
            transform.position = new Vector2(InGameVars.LevelConfigs.m_levelMinX, transform.position.y);
        }
        else
        if (transform.position.x > InGameVars.LevelConfigs.m_levelMaxX)
        {
            transform.position = new Vector2(InGameVars.LevelConfigs.m_levelMaxX, transform.position.y);
        }
        if (transform.position.x < GameManager.mainCamera.transform.position.x - InGameVars.ScreenWidth / 2)
        {
            transform.position = new Vector2(GameManager.mainCamera.transform.position.x - InGameVars.ScreenWidth / 2, transform.position.y);
        }
        else
        if (transform.position.x > GameManager.mainCamera.transform.position.x + InGameVars.ScreenWidth / 2)
        {
            transform.position = new Vector2(GameManager.mainCamera.transform.position.x + InGameVars.ScreenWidth / 2, transform.position.y);
        }
    }

    // 时间到了回调
    public void OnTimeOver()
    {
        m_wontBeHurt = false;
        GetDamage(new Damage(m_HP)); //直接死掉
    }

    //受击后的行动
    void OnDmgAction()
    {
        physicsObject.velocity.y = m_hurtJumpVelocityY; //给个Y速度
        MoveX(-1); //后跳
        m_wontBeHurt = true; //无敌
        ActiveFlicker(true); //闪烁
        new EnumTimer(() =>
        { //无敌时间
            m_wontBeHurt = false;
            ActiveFlicker(false);
        }, m_wontBeHurtTime).StartTimeout(this);
    }

    // 掉坑死
    void CheckDropDie()
    {
        if (m_isDead) return;
        if (CheckDropOutScreen())
        {
            m_isDead = true;
            new EnumTimer(() =>
            { //无敌时间
                OnDied?.Invoke(m_diedDamage);
            }, 1.5f).StartTimeout(this);

        }
    }

    // 当前Y高度
    float GetJumpHeight()
    {
        return physicsObject.distanceToGound;
    }

    // 是否在地上的行动
    bool CheckIsOnGroundActions()
    {
        return CheckGrounded() && m_status == SimonStatus.Idle || (m_status == SimonStatus.Walk) || (m_status == SimonStatus.Squat);
    }

    // 没有攻击中
    bool CheckNotAttacking()
    {
        return m_status != SimonStatus.MeleeAttack && m_status != SimonStatus.SquatMeleeAttack && m_status != SimonStatus.SubWeaponAttack;
    }

    // 可以攻击（不包括蹲着）
    bool CheckCanAttack()
    {
        return CheckNotAttacking() && (GetJumpHeight() > m_SpriteYOffset1 || GetJumpHeight() < 0.1);//太低了不能攻击//避免碰撞box卡进去
    }

    /*** 下面是 Action 区域 ****/

    //发射副武器
    public void SubWeaponAttack()
    {
        if (!m_subWeaponShooter.CanShoot) return;
        if (!CheckCanAttack()) return;
        ChangeState(SimonStatus.SubWeaponAttack);
    }

    //等待
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

    //蹲着转向
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

    //攻击
    public void MeleeAttack()
    {
        if (CheckCanAttack())
        {
            ChangeState(SimonStatus.MeleeAttack);
            if (CheckGrounded())
            {
                StopX();
            }
        }
    }

    //蹲攻击
    public void SquatMeleeAttack()
    {

        if (m_status != SimonStatus.Squat) return;
        if (CheckNotAttacking())
        {
            ChangeState(SimonStatus.SquatMeleeAttack);
            StopX();
        }
    }

    //走路
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

    //蹲着
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

    //跳跃
    public void Jump(Vector2 direction)
    {
        if (m_status == SimonStatus.Squat) return;
        if (CheckNotAttacking())
            if (CheckIsOnGroundActions())
            {
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


    /*** 上面是 Action 区域 ****/
}