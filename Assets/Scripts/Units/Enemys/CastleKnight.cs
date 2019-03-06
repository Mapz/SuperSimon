using BTAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CastleKnight : SimpleEnemy
{
    private int m_isAwake = -1;
    public WeaponShooter m_cannonShooter;
    private float m_jumpVelocityX;
    private float m_backJumpTime = 1.5f;

    protected override void OnEnabled()
    {
        base.OnEnabled();

    }


    public override void AddAI()
    {
        base.AddAI();
        ai = BT.Root();
        ai.OpenBranch(
               BT.If(() => { return m_isAttacked; }).OpenBranch(BT.If(() => { return -1 == m_isAwake; }).OpenBranch(BT.Call(() =>
                    {
                        m_isAwake = 0;
                        AttachToHPBar(GameManager.StatusBar.m_enemyHp);
                        m_anim.SetTrigger("WakeUp");
                    }))),
               BT.If(() => { return 1 == m_isAwake; }).OpenBranch( //已經激活
                    BT.If(() =>
                    {
                        var distanceToHero = this.CheckDistanceX(InGameVars.hero);
                        //Debug.Log("distanceToHero 1:" + distanceToHero);
                        return distanceToHero < 70;  // 太近，跳遠一點
                    }).OpenBranch(BT.If(CheckGrounded).OpenBranch(

                        BT.Random().OpenBranch(
                            //BT.Sequence().OpenBranch(
                            //        BT.Call(() =>
                            //             {
                            //                 Debug.Log("Too Near");
                            //                 //StopX();
                            //                 //JumpToScreenEdge();
                            //                 m_anim.SetTrigger("WalkTrigger");
                            //                 MoveX(-1);
                            //             }),
                            //        BT.Wait(3f),
                            //        BT.Call(Idle)
                            //    ),
                            BT.Sequence().OpenBranch(
                                  BT.Call(Idle),
                                  BT.Wait(0.1f),
                                  BT.Call(() =>
                                  {
                                      //Debug.Log("Too Near");
                                      StopX();
                                      JumpToScreenEdge();
                                      //m_anim.SetTrigger("WalkTrigger");
                                      //MoveX(-1);
                                  }),
                                  BT.Wait(m_backJumpTime),
                                  BT.Call(Idle),
                                  BT.Wait(0.1f),
                                  BT.Call(() =>
                                  {
                                      m_anim.SetTrigger("FireTrigger");
                                      //m_anim.Play("FireCanon");
                                      m_jumpVelocityX = 0;
                                      StopX();
                                  }),
                                  BT.WaitForAnimatorState(m_anim, "Idle")
                                //BT.Call(Idle)

                                )
                            )
                        )





                    ),
                    BT.If(() =>
                    {
                        var distanceToHero = this.CheckDistanceX(InGameVars.hero);
                        //Debug.Log("distanceToHero 0:" + distanceToHero);
                        return distanceToHero > 90;
                    }).OpenBranch(
                            //BT.Trigger(m_anim, "AxeAttackTrigger", false),
                            BT.Call(Walk),
                            BT.Wait(0.1f),
                            BT.Trigger(m_anim, "WalkTrigger", false)
                        ),
                    BT.If(() =>
                    {
                        var distanceToHero = this.CheckDistanceX(InGameVars.hero);
                        //Debug.Log("distanceToHero 2:" + distanceToHero);
                        return distanceToHero > 70 && distanceToHero < 90;
                    }).OpenBranch(
                                BT.Call(Idle),
                                BT.Wait(0.1f),
                                BT.Call(() =>
                                    {
                                        m_anim.Play("AxeAttack");
                                    }
                                ),
                                BT.WaitForAnimatorState(m_anim, "Idle")

                            )
                  )
        );
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
        var ToX = CameraPositionX + (InGameVars.ScreenWidth / 2 - 80) * ((facingRight) ? -1 : 1);
        var JumpFar = ToX - transform.position.x;

        //通过时间来计算Y 速度
        physicsObject.velocity.y = -m_backJumpTime / 2 * physicsObject.gravityModifier * Physics2D.gravity.y;
        //通过时间来计算X 速度
        m_jumpVelocityX = JumpFar / m_backJumpTime;
        MoveX(-1);
        m_anim.SetTrigger("JumpTrigger");

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
            Flip();
        }
    }

    public void WakeUpOver()
    {
        m_isAwake = 1;
    }

    public void Idle()
    {
        m_anim.ResetTrigger("WalkTrigger");
        m_anim.SetTrigger("IdleTrigger");
        StopX();
    }

    public void Walk()
    {
        m_anim.SetTrigger("WalkTrigger");
        MoveX();
    }
}
