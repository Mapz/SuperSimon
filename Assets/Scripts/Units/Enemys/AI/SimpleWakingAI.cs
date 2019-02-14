using BT;
using UnityEngine;

public class SimpleWakingAI : BTTree {

    protected override void Init () {
        base.Init ();

        // BTConfiguration.ENABLE_BTACTION_LOG = true;
        BTConfiguration.ENABLE_DATABASE_LOG = true;

        // root 是优先选择器
        _root = new BTSequence ();
        _root.interval = 0.1f;
        BTSequence ifCollide = new BTSequence (new CheckCollisionWithGroundAndContactEnemy ()); {
            ifCollide.AddChild (new AIActionFlip ());
        }
        _root.AddChild (new ActionMove ());
        _root.AddChild (ifCollide);

    }
}