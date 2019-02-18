using UnityEngine;
public struct CheckPointData
{
    public Vector2 Position;
    public int CurrentLevel;
}

public static class CheckPointStatus
{
    private static CheckPointData m_checkPoint = new CheckPointData();

    public static void SetCheckPoint(Vector2 position)
    {
        m_checkPoint.Position = position;
        m_checkPoint.CurrentLevel = InGameVars.stage;
    }

    public static CheckPointData GetCheckPoint()
    {
        if (InGameVars.stage != m_checkPoint.CurrentLevel)
        {
            m_checkPoint.Position = InGameVars.LevelConfigs.m_StartPos;
            m_checkPoint.CurrentLevel = InGameVars.stage;
        }
        return m_checkPoint;
    }
}
