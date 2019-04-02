using UnityEngine;

public static class UnitHelper
{
    public static float CheckDistance(this Unit unit, Unit other)
    {
        if (other == null) return 4000f;
        return (new Vector2(unit.transform.position.x, unit.transform.position.y) - new Vector2(other.transform.position.x, other.transform.position.y)).magnitude;
    }

    public static float CheckDistanceX(this Unit unit, Unit other)
    {
        if (other == null) return 4000f;
        return Mathf.Abs(unit.transform.position.x - other.transform.position.x);
    }

    public static bool isAtRightOf(this Unit unit, Unit other, bool defaultValue = true)
    {
        if (other == null) return defaultValue;
        return (unit.transform.position - other.transform.position).x > 0;
    }

    public static bool isNearScreenHEdge(this Unit unit, float acceptableRange = 30)
    {
        return InGameVars.ScreenWidth / 2 - Mathf.Abs(GameManager.mainCamera.transform.position.x - unit.transform.position.x) < acceptableRange;
    }
}
