using BT;
using UnityEngine;

public class SimpleWakingAI : BTTree
{

    /**
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */
    protected override void Init()
    {
        base.Init();
        BTConfiguration.ENABLE_DATABASE_LOG = true;

        // root 是优先选择器
        _root = new BTPrioritySelector();
        _root.interval = 0.1f;

        BTSequence ifInit = new BTSequence(new CheckInited());
        {
            ifInit.AddChild(new AIActionFlip());
        }

        BTSequence ifCollide = new BTSequence(new CheckCollided());
        {
            ifCollide.AddChild(new AIActionFlip());
        }
        _root.AddChild(ifInit);
        _root.AddChild(new ActionMove());
        _root.AddChild(ifCollide);

    }
}