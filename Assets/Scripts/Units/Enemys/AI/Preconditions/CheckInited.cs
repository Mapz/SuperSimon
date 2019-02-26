using System;
using BT;

public class CheckInited : BTPrecondition
{

    public CheckInited()
    {

    }

    public override void Activate(Database database)
    {
        base.Activate(database);
        database.SetData("Inited", false);
    }

    public override bool Check()
    {
        if (database.GetData<Boolean>("Inited"))
        {
            return false;
        }
        else
        {
            database.SetData("Inited", true);
            return true;
        }

    }
}
