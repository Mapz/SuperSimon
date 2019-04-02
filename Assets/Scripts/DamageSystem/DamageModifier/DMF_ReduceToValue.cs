using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMF_ReduceToValue : DamageModifier
{
    public float m_reduceTo = 1;
    public override float GetRealDmg(float dmg, DmgType dmgType)
    {
        if (dmg > m_reduceTo) return m_reduceTo;
        return dmg;
    }

}
