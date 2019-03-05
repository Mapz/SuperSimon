using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera m_camera;
    private float m_cameraMinX;
    private float m_cameraMaxX;
    public bool m_follows = true;
    void Start()
    {
        Init();
    }

    void Init()
    {
        //Call When LoadLevel
        m_cameraMinX = InGameVars.ScreenWidth / 2 + InGameVars.LevelConfigs.m_levelMinX;
        m_cameraMaxX = InGameVars.LevelConfigs.m_levelMaxX - InGameVars.ScreenWidth / 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!m_follows) return;
        var playerX = transform.position.x;
        var newLevelMinX = playerX - InGameVars.ScreenWidth / 2;
        if (newLevelMinX >= InGameVars.LevelConfigs.m_levelMinX)
        {
            InGameVars.LevelConfigs.m_levelMinX = newLevelMinX;
            m_cameraMinX = InGameVars.ScreenWidth / 2 + InGameVars.LevelConfigs.m_levelMinX;
        }
        if (playerX < m_cameraMinX)
        {
            m_camera.transform.position = new Vector3(m_cameraMinX, m_camera.transform.position.y, m_camera.transform.position.z);
        }
        else if (playerX > m_cameraMaxX)
        {
            m_camera.transform.position = new Vector3(m_cameraMaxX, m_camera.transform.position.y, m_camera.transform.position.z);
        }
        else
        {
            m_camera.transform.position = new Vector3(playerX, m_camera.transform.position.y, m_camera.transform.position.z);
        }

    }
}