using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameStateInGame : IGameState {

    private Unit Hero;

    private bool m_isPaused = false;

    private string m_levelPrefabName;

    public override void OnEnter (StatusParams param = null) {
        //Init A Game
        object Init = param.Get ("Init");
        //Level PrefabName
        if (param.Get<string> ("Level") != null) {
            m_levelPrefabName = param.Get<string> ("Level");
        }
        if (Init != null) {
            if ((bool) Init) {
                InitLevel ();
            }
        }

        //Set StartButton
        GameManager.input.m_start += PauseGame;

    }

    void InitLevel (bool loadUI = true) {
        if (loadUI) {
            GameManager.StatusBar = Utility.CreateUI ("UIGameState").GetComponent<StatusBar> ();
            GameManager.StatusBar.transform.SetParent (GameManager.UICanvas.transform, false);
        }
        // Load Level1
        InGameVars.level = Utility.LoadLevel (m_levelPrefabName);;
        // Load Hero
        Hero = Utility.CreateUnit ("Simon");
        Hero.SetFollowByCamera (GameManager.mainCamera);
        Hero.AttachToHPBar (GameManager.StatusBar.m_heroHp);
        Hero.transform.SetParent (InGameVars.level.transform);
        Hero.transform.position = InGameVars.level.GetComponent<LevelConfigs> ().m_StartPos;
        Hero.EnableInput ();
        InGameVars.hero = Hero;
        GameManager.CountDown.AttachToStatusBar (GameManager.StatusBar);
        GameManager.CountDown.StartCountDown (InGameVars.time);
        GameManager.StatusBar.Refresh ();
        Hero.OnDied += HeroDied;
    }

    void Clear () {
        GameObject.Destroy (InGameVars.level);
    }

    void RestartLevel () {
        Clear ();
        new EnumTimer (() => {
            InitLevel (false);
        }, 1f).StartTimeout ();
    }

    public void PauseGame () {
        m_isPaused = !m_isPaused;
        if (m_isPaused) {
            Time.timeScale = 0;
            Hero.DisableInput ();
        } else {
            Time.timeScale = 1;
            Hero.EnableInput ();
        }
    }

    public override void OnLeave () {
        GameManager.input.m_start -= PauseGame;
        Hero.OnDied -= HeroDied; // 如果只有 hero died 才离开 INGame状态，那么这个就没有意义
    }

    void HeroDied (Damage dmg) {
        InGameVars.life--;
        if (InGameVars.life == 0) {
            //TODO: GameOver Logic
        }
        RestartLevel ();
    }

}