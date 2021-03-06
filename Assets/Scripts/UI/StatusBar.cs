﻿using UnityEngine;
using UnityEngine.UI;
public class StatusBar : MonoBehaviour
{
    public Text m_score;
    public Text m_heart;
    public Text m_time;
    public Text m_stage;
    public HPBar m_heroHp;
    public HPBar m_enemyHp;
    public Text m_life;
    public Image m_Subewapon;

    public void SetLife(int life)
    {
        m_life.text = "P-" + life.ToString("D2");
    }

    public void SetScore(int score)
    {
        m_score.text = "SCORE-" + score.ToString("D6");
    }

    public void SetTime(int time)
    {
        m_time.text = "TIME  " + time.ToString("D4");
    }

    public void SetStage(int stage)
    {
        m_stage.text = "STAGE " + stage.ToString("D2");
    }

    public void SetHeart(int heart)
    {
        m_heart.text = "-" + heart.ToString("D2");
    }

    public void SetSubWeapon(Sprite subWeaponIcon)
    {
        if (subWeaponIcon == null)
        {
            m_Subewapon.gameObject.SetActive(false);
        }
        else
        {
            m_Subewapon.gameObject.SetActive(true);
            m_Subewapon.sprite = subWeaponIcon;
        }
    }

    public void Refresh()
    {
        InGameVars.heart = InGameVars.heart;
        InGameVars.hp = InGameVars.hp;
        InGameVars.life = InGameVars.life;
        InGameVars.score = InGameVars.score;
        InGameVars.stage = InGameVars.stage;
        InGameVars.time = InGameVars.time;
        SetSubWeapon(null); //假定死了一定丢副武器


    }
}