using BTAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CastleKnight : SimpleEnemy
{
    private int m_isAwake = -1;
    public WeaponShooter m_cannonShooter;
    private float m_jumpVelocityX;
    private float m_backJumpTime = 1.5f;
    public GameObject m_onHitPrefab;


    protected override void OnDmg(Damage dmg)
    {
        base.OnDmg(dmg);
        GameObject go = Instantiate(m_onHitPrefab);
        //Debug.Log("DmgPos:" + dmg.DmgPosition);
        go.transform.position = dmg.DmgPosition;
        go.transform.SetParent(InGameVars.level.transform, true);

    }

    protected override void UpdateSpeedX()
    {
        if (m_moving != 0)
        {
            if (m_jumpVelocityX != 0)
            {
                physicsObject.targetVelocity.x = m_moving * m_jumpVelocityX * (facingRight ? 1 : -1);
            }
            else
            {
                physicsObject.targetVelocity.x = m_moving * m_xMoveSpeed * (facingRight ? 1 : -1);
            }
        }

    }

    protected override void Die(Damage dmg)
    {
        m_isDead = true;
        m_sm.TransState("Died");
        var weapons = GetComponentsInChildren<Weapon>();
        foreach (var weapon in weapons)
        {
            Destroy(weapon);
        }
        transform.position += Vector3.up * 40;
        m_anim.PlayAvoidRePlay("Sleep");
        StopX();

    }

    public void ShootCanon()
    {
        Debug.Log("ShootCanon");
        if (!m_cannonShooter) return;
        m_cannonShooter.ExecShoot();
    }


    private void JumpToScreenEdge()
    {
        //板邊位置
        var CameraPositionX = GameManager.mainCamera.transform.position.x;
        var ToX = CameraPositionX + (InGameVars.ScreenWidth / 2 - 50) * ((facingRight) ? -1 : 1);
        var JumpFar = ToX - transform.position.x;

        //通过时间来计算Y 速度
        physicsObject.velocity.y = -m_backJumpTime / 2 * physicsObject.gravityModifier * Physics2D.gravity.y;
        //通过时间来计算X 速度
        m_jumpVelocityX = JumpFar / m_backJumpTime;
        MoveX(-1);
        m_anim.PlayAvoidRePlay("Jump");

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        //鎖定Player方向
        FaceToHero();
    }

    public void FaceToHero()
    {
        if (this.isAtRightOf(InGameVars.hero) == facingRight)
        {
            if (Math.Abs(transform.position.x - InGameVars.hero.transform.position.x) > 30)
                Flip();
        }
    }

    public void WakeUpOver()
    {
        m_isAwake = 1;
    }

    public void Idle()
    {
        m_sm.TransState("Idle");
    }

    public void Walk(int direction)
    {
        m_anim.PlayAvoidRePlay("Walk");
        MoveX(direction);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        RestrictPosition();
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


    public override void AddAI()
    {
        m_sm = new StateMachine();
        string stateNameSleep = "Sleep";
        string stateNameWakeUp = "WakeUp";
        string stateNameIdle = "Idle";
        string stateNameWalking = "Walking";
        string stateNameAxeAttack = "AxeAttack";
        string stateNameCannonAttack = "CannonAttack";
        string stateNameJumpBack = "JumpBack";
        string StateNameDied = "Died";
        float axeAttackCoolDown = 6f;
        float cannonAttackCoolDown = 3f;
        float WalkCoolDown = 3f;

        onStateAction distanceCheck = _ =>
        {
            float distance = this.CheckDistanceX(InGameVars.hero);
            var transSucceed = false;
            if ((distance > 80 || distance < 120))
            {
                transSucceed = m_sm.TransState(stateNameAxeAttack);
                if (transSucceed)
                {
                    return;
                }
                else
                {

                    m_sm.SetData("WalkingBack", true);
                    m_sm.SetData("distance", distance);
                    var walkSucceed = m_sm.TransState(stateNameWalking);
                    if (!walkSucceed)
                    {
                        m_sm.SetData("WalkingBack", false);
                    }
                }
            }

            if (distance < 80 || (distance > 120 && distance < 160))
            {

                if (distance < 80)
                {
                    var random = UnityEngine.Random.Range(0f, 1f);
                    Debug.Log(random);
                    if (random > 0.5f)
                    {
                        var jumpSucceed = m_sm.TransState(stateNameJumpBack);
                        if (jumpSucceed)
                            return;
                    }
                    else
                    {
                        m_sm.SetData("distance", distance);
                        m_sm.SetData("WalkingBack", true);
                        var walkSucceed = m_sm.TransState(stateNameWalking);
                        if (!walkSucceed)
                        {
                            m_sm.SetData("WalkingBack", false);
                        }
                    }
                }
                else
                {
                    m_sm.SetData("distance", distance);
                    m_sm.SetData("WalkingBack", false);
                    m_sm.TransState(stateNameWalking);
                }
            }
            if (distance > 160)
            {
                if (!m_sm.TransState(stateNameCannonAttack))
                {
                    m_sm.SetData("distance", distance);
                    m_sm.SetData("WalkingBack", false);
                    m_sm.TransState(stateNameWalking);
                }
            }
        };

        SMState stateDied = new SMState(StateNameDied);
        m_sm.AddState(stateDied);


        SMState stateSleep = new SMState(stateNameSleep);
        m_sm.AddState(stateSleep);
        stateSleep.OnEnterState = (_) =>
        {
            m_wontBeHurt = true;
        };

        stateSleep.OnExitState = (_) =>
        {
            m_wontBeHurt = false;
        };

        stateSleep.OnUpdate = (_) =>
        {
            if (m_isAttacked)
            {
                if (m_anim.IsAnimationState(stateNameSleep))
                {
                    m_sm.TransState(stateNameWakeUp);
                    m_anim.PlayAvoidRePlay("Awake");
                }
            }
        };

        // 起身
        SMState stateWakeUp = new SMState(stateNameWakeUp);
        m_sm.AddState(stateWakeUp);

        stateWakeUp.OnEnterState = _ =>
        {
            AttachToHPBar(GameManager.StatusBar.m_enemyHp);
            GameManager.cameraHandler.DisFollow();
        };
        stateWakeUp.OnUpdate = _ =>
        {
            if (m_isAwake == 1)
            {
                m_sm.TransState(stateNameIdle);
            }
        };




        SMState StateJumpBack = new SMState(stateNameJumpBack);
        m_sm.AddState(StateJumpBack);

        StateJumpBack.TransPreCheck = () =>
        {
            return m_sm.GetData<bool>("WalkCoolDown");
        };
        StateJumpBack.OnExitState = _ =>
        {
            m_jumpVelocityX = 0;
        };
        StateJumpBack.OnEnterState = _ =>
        {
            JumpToScreenEdge();
            new EnumTimer(() =>
            {
                m_sm.TransState(stateNameCannonAttack);
            }, m_backJumpTime).StartTimeout();
        };

        // 站立状态
        SMState StateIdle = new SMState(stateNameIdle);
        m_sm.AddState(StateIdle);
        StateIdle.OnEnterState = _ =>
        {
            m_anim.PlayAvoidRePlay("Idle");
            StopX();
        };


        StateIdle.OnUpdate = distanceCheck;

        SMState stateWalking = new SMState(stateNameWalking);
        m_sm.AddState(stateWalking);

        stateWalking.TransPreCheck = () =>
        {
            return m_sm.GetData<bool>("WalkCoolDown");
        };

        stateWalking.canTranlateToSelf = true;
        stateWalking.OnEnterState = _ =>
        {
            var distance = m_sm.GetData<float>("distance");
            var walkingBack = m_sm.GetData<bool>("WalkingBack");
            if (distance < 80 || walkingBack)
            {
                Walk(-1);
            }
            else
            {
                Walk(1);
            }

            m_sm.SetData("WalkCoolDown", false);
            new EnumTimer(() =>
            {
                m_sm.SetData("WalkCoolDown", true);
                m_sm.SetData("WalkingBack", false);
            }, WalkCoolDown).StartTimeout();

            return;
        };
        stateWalking.OnUpdate = state =>
        {

            if (!m_sm.GetData<bool>("WalkCoolDown"))
            {
                return;
            }
            distanceCheck.Invoke(state);
        };

        SMState stateAxeAttack = new SMState(stateNameAxeAttack);
        m_sm.AddState(stateAxeAttack);
        stateAxeAttack.OnEnterState = _ =>
        {
            StopX();
            m_anim.PlayAvoidRePlay("AxeAttack");
            m_sm.SetData("AxeAttackCoolDown", false);
            new EnumTimer(() =>
            {
                m_sm.SetData("AxeAttackCoolDown", true);
            }, axeAttackCoolDown).StartTimeout();
        };

        stateAxeAttack.TransPreCheck = () =>
        {
            return m_sm.GetData<bool>("AxeAttackCoolDown");
        };

        SMState stateCannonAttack = new SMState(stateNameCannonAttack);
        m_sm.AddState(stateCannonAttack);
        stateCannonAttack.OnEnterState = _ =>
        {
            StopX();
            m_anim.PlayAvoidRePlay("FireCanon");
            m_sm.SetData("CannonAttackCoolDown", false);
            new EnumTimer(() =>
            {
                m_sm.SetData("CannonAttackCoolDown", true);
            }, cannonAttackCoolDown).StartTimeout();
        };

        stateCannonAttack.TransPreCheck = () =>
        {
            return m_sm.GetData<bool>("CannonAttackCoolDown");
        };


        m_sm.TransState(stateNameSleep);
        m_sm.SetData("AxeAttackCoolDown", true);
        m_sm.SetData("CannonAttackCoolDown", true);
        m_sm.SetData("WalkCoolDown", true);

    }

}
