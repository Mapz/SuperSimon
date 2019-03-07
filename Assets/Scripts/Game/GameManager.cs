using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public static class GameManager
{

    public static bool inited = false;

    private static Camera _mainCamera;
    public static Camera mainCamera
    {
        get
        {
            return _mainCamera;
        }
        set { _mainCamera = value; }
    }

    public static CameraHandler cameraHandler
    {
        get
        {
            return _mainCamera.GetComponent<CameraHandler>();
        }

    }




    private static Canvas _UICanvas;
    public static Canvas UICanvas
    {
        get
        {
            return _UICanvas;
        }
        set { _UICanvas = value; }
    }

    private static Canvas _InGameUICanvas;
    public static Canvas InGameUICanvas
    {
        get
        {
            return _InGameUICanvas;
        }
        set { _InGameUICanvas = value; }
    }

    private static PixelPerfectCamera _ppCamera;
    public static PixelPerfectCamera ppCamera
    {
        get
        {
            return _ppCamera;
        }
        set { _ppCamera = value; }
    }

    private static InputSystem _input;
    public static InputSystem input
    {
        get
        {
            return _input;
        }
        set { _input = value; }
    }

    private static StatusBar _StatusBar;
    public static StatusBar StatusBar
    {
        get
        {
            return _StatusBar;
        }
        set { _StatusBar = value; }
    }

    private static Game _Game;
    public static Game Game
    {
        get
        {
            return _Game;
        }
        set { _Game = value; }
    }

    private static CountDown _CountDown;
    public static CountDown CountDown
    {
        get
        {
            return _CountDown;
        }
        set { _CountDown = value; }
    }

    public static Grid tileGrid;

    public static Font MainFont;

}