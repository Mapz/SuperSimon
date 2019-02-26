using UnityEngine;

public partial class Utility
{
    public static Vector2 CheckOutOfScreen(Vector2 position, int outOffset = 0)
    {
        var direction = Vector2.zero;
        if (position.y < GameManager.mainCamera.transform.position.y - InGameVars.ScreenHeight / 2 - outOffset)
        {
            direction = Vector2.down;

        }
        else
        if (position.y > GameManager.mainCamera.transform.position.y + InGameVars.ScreenHeight / 2 + outOffset)
        {
            direction = Vector2.up;

        }
        else
        if (position.x < GameManager.mainCamera.transform.position.x - InGameVars.ScreenWidth / 2 - outOffset)
        {
            direction = Vector2.left;

        }
        else
        if (position.x > GameManager.mainCamera.transform.position.x + InGameVars.ScreenWidth / 2 + outOffset)
        {
            direction = Vector2.right;

        }

        return direction;
    }

    public static bool CheckInSpawnArea(Vector2 position, int outOffset = 30)
    {
        Bounds outter = new Bounds(new Vector2(GameManager.mainCamera.transform.position.x, GameManager.mainCamera.transform.position.y), new Vector2(InGameVars.ScreenWidth + outOffset, InGameVars.ScreenHeight + +outOffset));
        return outter.Contains(position);  //inner.Contains(position);
    }
}