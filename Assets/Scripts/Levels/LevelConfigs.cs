using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfigs : MonoBehaviour {
    public float m_levelMinX;
    public float m_levelMaxX;
    public Vector2 m_StartPos;

    void Awake () {
        InGameVars.LevelConfigs = this;
    }

}