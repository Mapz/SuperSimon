using System.Collections;
using UnityEngine;
public class GameStateInGame : IGameState
{

    private Unit Hero;

    private bool m_isPaused = false;

    private string m_levelPrefabName;

    public override void OnEnter(StatusParams param = null)
    {
        //Init A Game
        object Init = param.Get("Init");
        //Level PrefabName
        if (param.Get<string>("Level") != null)
        {
            m_levelPrefabName = param.Get<string>("Level");
        }
        if (Init != null)
        {
            if ((bool)Init)
            {
                InitLevel();
            }
        }

        //Set StartButton
        GameManager.input.m_start += PauseGame;

    }

    void InitLevel(bool loadUI = true)
    {
        if (loadUI)
        {
            GameManager.StatusBar = Utility.CreateUI("UIGameState").GetComponent<StatusBar>();
            GameManager.StatusBar.transform.SetParent(GameManager.UICanvas.transform, false);
        }

        //InGameVars.heart = 300;//DEBUG

        InGameVars.time = 300; //TODO 改成从关卡读取
        // Load Level1
        InGameVars.level = Utility.LoadLevel(m_levelPrefabName);
        // Load Hero
        Hero = Utility.CreateUnit("Simon");

        Hero.SetFollowByCamera(GameManager.mainCamera);
        Hero.AttachToHPBar(GameManager.StatusBar.m_heroHp);
        StartCoroutine(SetHeroPosition());

        Hero.EnableInput();
        InGameVars.hero = Hero;
        GameManager.CountDown.AttachToStatusBar(GameManager.StatusBar);

        GameManager.CountDown.StartCountDown(InGameVars.time);
        GameManager.StatusBar.Refresh();

        GameManager.CountDown.OnTimeOut += ((Simon)Hero).OnTimeOver;

        Hero.OnDied += HeroDied;
    }

    IEnumerator SetHeroPosition()
    {
        if (Hero == null) yield break;
        yield return new WaitForFixedUpdate();
        Hero.transform.SetParent(InGameVars.level.transform);
        Hero.transform.position = CheckPointStatus.GetCheckPoint().Position;
        RaycastHit2D groundPointHit = Physics2D.Raycast(Hero.transform.position, Vector2.down, 256f, 1 << LayerMask.NameToLayer("Ground"));
        if (groundPointHit.collider)
        {
            var groundPoint = groundPointHit.point;
            Hero.transform.position = Vector2.up + new Vector2(groundPoint.x, groundPoint.y - (Hero.GetComponent<SpriteRenderer>().size.y - Hero.GetComponent<SpriteRenderer>().size.y - Hero.GetComponent<SpriteRenderer>().sprite.pivot.y));
        }
        if (groundPointHit.collider != null)
        {
            Debug.DrawLine(Hero.transform.position, groundPointHit.point, Color.red, 100.1f, false);

        }

    }

    void Clear()
    {
        GameObject.Destroy(InGameVars.level);
    }

    void RestartLevel()
    {
        Clear();
        new EnumTimer(() =>
        {
            InitLevel(false);
        }, 1f).StartTimeout();
    }

    public void PauseGame()
    {
        m_isPaused = !m_isPaused;
        if (m_isPaused)
        {
            Time.timeScale = 0;
            Hero.DisableInput();
        }
        else
        {
            Time.timeScale = 1;
            Hero.EnableInput();
        }
    }

    public override void OnLeave()
    {
        GameManager.input.m_start -= PauseGame;
        Hero.OnDied -= HeroDied; // 如果只有 hero died 才离开 INGame状态，那么这个就没有意义
    }

    void HeroDied(Damage dmg)
    {
        GameManager.CountDown.OnTimeOut -= ((Simon)Hero).OnTimeOver;
        GameManager.CountDown.Stop();
        InGameVars.life--;
        if (InGameVars.life == 0)
        {
            //TODO: GameOver Logic
        }
        RestartLevel();
    }

    void HeroDied()
    {

    }

}