using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMF_Mutipler : DamageModifier
{
    public float Magnification;


    public override float GetRealDmg(float dmg, DmgType dmgType)
    {
        var raw = dmg - m_onHitBox.m_defence;
        return raw * Magnification;
    }
}
