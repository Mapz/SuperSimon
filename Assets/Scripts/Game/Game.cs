using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance;
    //DONE: 场景空气墙  done
    //DONE: 场景摄像机跟随人物X轴  done
    //DONE: Simon 蹲下打击后，恢复行走的逻辑按键不正确 done
    //DONE: 顶砖块逻辑修正 done
    //DONE: 修复Simon攻击后停止的问题 done
    //DONE: 生Coin逻辑增加 done
    //DONE: 拖板逻辑 done
    //DONE: 人物走不动的BUG done -->  TilemapCollider 需要和 compositeCollider 一起使用，避免毛刺和卡顿
    //DONE: 使用compositeCollider后，判断撞击的问题 done --> 使用 BoxCollider.size 来解决 Size 的获得问题
    //DONE: 受击CD判定的实现 done --> 使用 EnumTimer 判定 Collider 对 Unit 的受击
    //DONE: 优化脚下判定的问题 done --> 使用 Sprite size 和 锚点来做射线判断，并且增加多个判断点，任然有优化空间，可以采用更改为Collider的方式
    //DONE: 被打的动画 done --> 可能会有未知bug，Simon 的逻辑还要优化 --> 闪烁无敌 Shader 实现的
    //DONE: 对敌打击后反伤的Bug done --> 原因是，如果在 Child 上，不加入 RigidBody ，那么他的 Collider 时间会上升到父节点
    //参考 https://answers.unity.com/questions/410711/trigger-in-child-object-calls-ontriggerenter-in-pa.html
    //DONE: 对敌打击生效的问题 done --> 思路：collider 只能生效一次，如果位置不动的话
    //解决： 不再使用 Enable Collider 的方式来处理，而是使用 GameObject SetActive 来打开武器
    //思考： 如果这个策略消耗过高，可以再使用抖动武器的 TransForm 的方式来解决
    //DONE: 稍微调窄Collider，人物可以漏下去 Done 
    //DONE: 一次掉了N格子血的问题 --> 是读取到 IngameVars.hp 的 BUG，修改了 IngameVars.hp 的 setter
    //DONE: Simon的受击移动应该是受击方向，而不是人物方向 -> 重建了伤害系统，增加了伤害类型，伤害方向等
    //DONE: 蘑菇死亡动画 -> 鞭子死亡 -> 重建了伤害系统，增加了伤害类型，伤害方向等，用DOtween实现了简单的
    //DONE: 掉坑死
    //DONE: 时间到死 --> 优化CountDown,然后直接使用一个伤害杀死了 --> 注意后期的伤害计算会死不掉



    //TODO: 重生逻辑 --> 关卡进度逻辑即是
    //TODO: 生怪器
    //TODO: GameOver 逻辑
    //TODO: 副武器系统
    //TODO: 数值配置化  --> 包括升级系统的改版
    //TODO: 顶砖块，到顶了再爆炸
    //TODO: 武器升级动画，包括其他东西的暂停系统
    //TODO: 滑步的问题
    //TODO: 移动中可以穿过格子-->移动中碰撞体加大一点

    void Awake () {
        Instance = this;
    }

    void Start () {
        GameStatus.ChangeState<GameStateInit> ();
    }

}