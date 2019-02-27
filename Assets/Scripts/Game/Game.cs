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
    //DONE: 重生逻辑 --> 关卡进度逻辑即是,CheckPointObject ，然後直接注意主角重生後要落在地上
    //DONE: 副武器系统 --> 使用的是MovingUnit+SubWeaponData+Weapon的组合，一个发射器，和枪械发射器类似的
    //DONE: 武器升级动画，包括其他东西的暂停系统 --> 使用了调色板shader和Time.timescale=0,并用unscaledtime或者Waitforsecondsrealtime来播放一组动画，
    //DONE: 滑步的问题，修改Stop横向移动逻辑
    //DONE: 优化移动逻辑 --> 不再使用Unity物理，改用个自定义物理,改用Kinematic方式来处理 Use Full Kinematic Contact 自定义物理逻辑
    //DONE: 跳矮墙的时候VelocityY为负的BUG --> 因为在不同Sprite修改Collider的时候，大小不一致，导致和墙面重叠了，自定义物理失效，修改为同样宽度的Collider即可
    //思考：此问题如何永久解决
    //DONE: 攻击武器一帧化 --> 新增找到动画clip的函数，取得framerate
    //DONE: 迴旋鏢完成
    //DONE: 更多副武器的完成 --> 圣水完成
    //DONE: 通过Collider变化来判断是否重叠解决物理重合挤开逻辑----> 发射副武器的位置Bug --> 位置太低的時候不准開火
    //DONE: 生怪器
    //DONE: Unit通过ObjectMgr来产生

    //TODO: 资源模式调通
    //TODO: 进入管子的逻辑
    //TODO: 跟随探索 Cinemachine 
    //TODO: 物理重合挤开逻辑
    //TODO: 时停副武器的制作
    //TODO: 分数系统的制作
    //TODO: 边沿探测和物理逻辑的契合
    //TODO: 多人在线探索
    //TODO: GameOver 逻辑
    //TODO: 设计一个Boss
    //TODO: 数值配置化  --> 包括升级系统的改版
    //TODO: 移动中可以穿过格子-->移动中碰撞体加大一点
    void Awake () {
        Instance = this;
    }

    void Start () {
        GameStatus.ChangeState<GameStateInit> ();
    }

}