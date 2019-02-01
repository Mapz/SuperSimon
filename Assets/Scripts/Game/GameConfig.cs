using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GameConfig : MonoBehaviour {
    public Font m_mainfont;
    public Camera m_mainCamera;
    public PixelPerfectCamera m_ppCamera;

    public InputSystem m_input;

    public Canvas m_canvas;
    public Canvas m_inGameCanvas;

    public Game m_game;
    void Awake () {
        Init ();
    }

    void Init () {
        GameManager.MainFont = m_mainfont;
        GameManager.ppCamera = m_ppCamera;
        GameManager.mainCamera = m_mainCamera;
        GameManager.UICanvas = m_canvas;
        GameManager.input = m_input;
        GameManager.InGameUICanvas = m_inGameCanvas;
        GameManager.Game = m_game;
        GameManager.CountDown = gameObject.GetComponent<CountDown> ();
        GameManager.inited = true;
    }

}