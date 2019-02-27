using BT;
using UnityEngine;

public class CheckOnAttack : BTPrecondition
{

    private MovingUnit unit;
    public CheckOnAttack()
    {

    }

    public override void Activate(Database database)
    {
        base.Activate(database);
        unit = database.GetComponent<MovingUnit>();

    }

    public override bool Check()
    {
        return unit.m_isAttacked;
    }

}