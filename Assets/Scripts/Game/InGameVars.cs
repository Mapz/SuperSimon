using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public static class InGameVars
{

    public const int MaxStage = 1;
    public static int ScreenWidth;
    public static int ScreenHeight;
    public static LevelConfigs LevelConfigs;
    public static GameObject level;
    private static int _heart;
    public static int heart
    {
        get { return _heart; }
        set
        {
            if (GameManager.StatusBar)
            {
                GameManager.StatusBar.SetHeart(value);
            }
            _heart = value;
        }
    }

    private static int _life = 3;
    public static int life
    {
        get { return _life; }
        set
        {
            if (GameManager.StatusBar)
            {
                GameManager.StatusBar.SetLife(value);
            }
            _life = value;
        }
    }

    public static float hp
    {
        get
        {
            if (InGameVars.hero)
            {
                return InGameVars.hero.m_HP;
            }
            else
            {
                return 0;
            }
        }
        set
        {

            if (InGameVars.hero)
            {
                InGameVars.hero.m_HP = value;
            }
        }
    }

    public static int score
    {
        get { return _score; }
        set
        {
            if (GameManager.StatusBar)
            {
                GameManager.StatusBar.SetScore(value);

            }
            _score = value;
        }
    }
    private static int _score = 0;

    public static int time
    {
        get { return _time; }
        set
        {
            if (GameManager.StatusBar)
            {
                GameManager.StatusBar.SetTime(value);

            }
            _time = value;
        }
    }
    private static int _time = 300;

    public static int stage
    {
        get { return _stage; }
        set
        {
            if (GameManager.StatusBar)
            {
                GameManager.StatusBar.SetStage(value);

            }
            _stage = value;
        }
    }
    private static int _stage = 1;

    public static Unit hero
    {
        get { return _hero; }
        set
        {
            _hero = value;
        }
    }
    private static Unit _hero;



    public static void Init()
    {
        if (GameManager.ppCamera == null)
        {
            throw new GameNotInitException("pp摄像机未创建");
        }
        ScreenWidth = GameManager.ppCamera.refResolutionX;
        ScreenHeight = GameManager.ppCamera.refResolutionY;
        heart = 0;
        hp = 16;
        score = 0;
        time = 300;
        stage = 1;
        life = 3;
    }

}