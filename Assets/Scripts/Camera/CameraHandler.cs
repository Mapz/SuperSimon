using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private Camera m_camera;
    private float m_cameraMinX;
    private float m_cameraMaxX;
    public GameObject m_followsObject;
    private bool m_isFollowing = false;
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    public void Init()
    {
        //Call When LoadLevel
        m_cameraMinX = InGameVars.ScreenWidth / 2 + InGameVars.LevelConfigs.m_levelMinX;
        m_cameraMaxX = InGameVars.LevelConfigs.m_levelMaxX - InGameVars.ScreenWidth / 2;
    }

    public void SetFollow(GameObject obj)
    {
        m_isFollowing = true;
        m_followsObject = obj;
    }

    public void DisFollow()
    {
        m_isFollowing = false;
    }

    void Follows()
    {
        if (!m_isFollowing) return;
        if (m_followsObject == null) return;
        var followsX = m_followsObject.transform.position.x;
        var newLevelMinX = followsX - InGameVars.ScreenWidth / 2;
        if (newLevelMinX >= InGameVars.LevelConfigs.m_levelMinX)
        {
            InGameVars.LevelConfigs.m_levelMinX = newLevelMinX;
            m_cameraMinX = InGameVars.ScreenWidth / 2 + InGameVars.LevelConfigs.m_levelMinX;
        }
        if (followsX < m_cameraMinX)
        {
            m_camera.transform.position = new Vector3(m_cameraMinX, m_camera.transform.position.y, m_camera.transform.position.z);
        }
        else if (followsX > m_cameraMaxX)
        {
            m_camera.transform.position = new Vector3(m_cameraMaxX, m_camera.transform.position.y, m_camera.transform.position.z);
        }
        else
        {
            m_camera.transform.position = new Vector3(followsX, m_camera.transform.position.y, m_camera.transform.position.z);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Follows();
    }
}