using BT;
using UnityEngine;

public class CheckCollided : BTPrecondition
{

    private MovingUnit unit;
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

        return false;

    }

}