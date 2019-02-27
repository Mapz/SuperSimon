using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAI : BTTree
{
    protected override void Init()
    {
        base.Init();
        BTConfiguration.ENABLE_DATABASE_LOG = true;
        BTConfiguration.ENABLE_BTACTION_LOG = true;

        _root = new BTPrioritySelector();
        //_root.interval = 0.1f;

        CheckOnAttack isAttacked = new CheckOnAttack();
        {
            isAttacked.AddChild(new AIActionGurad());
        }

        CheckInited ifInit = new CheckInited();
        {
            ifInit.AddChild(new AIActionFlip());
        }

        CheckCollided ifCollide = new CheckCollided();
        {
            ifCollide.AddChild(new AIActionFlip());
        }
        _root.AddChild(ifInit); // 先初始化
        _root.AddChild(isAttacked);//检查是否被打了
        _root.AddChild(ifCollide); //检测是否碰撞了
        _root.AddChild(new ActionMove()); //不然移动
       

    }
}
