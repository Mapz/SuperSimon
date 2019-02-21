using BT;
using UnityEngine;

public class CheckCollided : BTPrecondition
{

    private MovingUnit unit;
    //TODO: 後續添加縱向碰撞的邏輯，現在只有橫向碰撞
    public CheckCollided()
    {

    }

    public override void Activate(Database database)
    {
        base.Activate(database);
        unit = database.GetComponent<MovingUnit>();

    }

    public override bool Check()
    {
        var pho = unit.GetComponent<PhysicsObject>();

        if (pho) //使用新的PhysicsObject
        {
            return pho.collided;
        }
        else
        {
            RaycastHit2D[] hits;
            Vector3 endPoint = (unit.facingRight ? 1 : -1) * Vector2.right * (unit.GetComponent<SpriteRenderer>().size.x / 2 + 2);
            hits = Physics2D.RaycastAll(unit.transform.position, (unit.facingRight ? 1 : -1) * Vector2.right, unit.GetComponent<SpriteRenderer>().size.x / 2 + 2, 1 << LayerMask.NameToLayer("EnemyCanContact") | 1 << LayerMask.NameToLayer("Ground"));
            Debug.DrawLine(unit.transform.position, unit.transform.position + endPoint, Color.green, 0.1f, false);
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider != unit.GetComponent<Collider2D>())
                {
                    Debug.DrawLine(unit.transform.position, hit.point, Color.red, 0.01f, false);
                    return true;
                }
            }
        }

        return false;

    }

}