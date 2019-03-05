using BTAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CastleKnight : SimpleEnemy
{
    private int m_isAwake = -1;



    public override void AddAI()
    {
        base.AddAI();
        ai = BT.Root();
        ai.OpenBranch(
           BT.If(() => { return m_isAttacked; }).
                OpenBranch(
                   BT.If(() => { return -1 == m_isAwake; }). // 被攻擊，未激活的時候激活
                    OpenBranch(BT.Call(() =>
                    {
                        m_isAwake = 0;
                        AttachToHPBar(GameManager.StatusBar.m_enemyHp);
                        m_anim.SetTrigger("WakeUp");
                    }))
                   ),
           BT.If(() => { return 1 == m_isAwake; }).OpenBranch( //已經激活
                BT.If(() =>
                {
                    var distanceToHero = this.CheckDistanceX(InGameVars.hero);
                    return distanceToHero > 70 && distanceToHero < 90;
                }).OpenBranch( // 範圍內，單體攻擊
                        BT.Call(() =>
                            {
                                Debug.Log("MeleeAttackRange");
                                m_anim.SetTrigger("AxeAttackTrigger");
                                StopX();
                            }),
                        BT.WaitForAnimatorState(m_anim, "Idle")
                    ),
                BT.If(() =>
                {
                    var distanceToHero = this.CheckDistanceX(InGameVars.hero);
                    Debug.Log("distanceToHero 0:" + distanceToHero);
                    return distanceToHero > 90;
                }).OpenBranch(  // 範圍外，靠近
                        BT.Call(() =>
                        {

                            Debug.Log("Too Far");
                            m_anim.SetTrigger("WalkTrigger");
                            MoveX();
                        }
                    ),
                BT.If(() =>
                {
                    var distanceToHero = this.CheckDistanceX(InGameVars.hero);
                    Debug.Log("distanceToHero 1:" + distanceToHero);
                    return distanceToHero < 70;  // 太近，跳遠一點
                }).OpenBranch( // 太近
                        BT.Call(() =>
                        {
                            Debug.Log("Too Near");
                            StopX();
                            JumpToScreenEdge();
                        }
                    )
                )
             )
          )
       );
    }


    private void JumpToScreenEdge()
    {
        //板邊位置
        var CameraPositionX = GameManager.mainCamera.transform.position.x;
        var ToX = CameraPositionX + (InGameVars.ScreenWidth / 2) * ((facingRight) ? -1 : 1);
        var JumpFar = ToX - transform.position.x;
        transform.DOJump(new Vector2(ToX, transform.position.y), 30, 1, 2f).OnComplete(Idle);
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
        m_anim.SetTrigger("IdleTrigger");
        StopX();
    }
}
