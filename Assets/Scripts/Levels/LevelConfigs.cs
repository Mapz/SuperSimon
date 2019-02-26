using UnityEngine;

public class LevelConfigs : MonoBehaviour {
    public float m_levelMinX;
    public float m_levelMaxX;
    public Vector2 m_StartPos;
    public Transform m_EnemyObjectParentTransform;
    public Grid m_levelGrid;

    void Awake () {
        InGameVars.LevelConfigs = this;
    }

}