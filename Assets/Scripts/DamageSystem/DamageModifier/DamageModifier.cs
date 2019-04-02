using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageModifier : MonoBehaviour
{
    protected OnHitBox m_onHitBox;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        m_onHitBox = GetComponent<OnHitBox>();
    }
    public virtual float GetRealDmg(float dmg, DmgType dmgType)
    {
        if (dmgType == DmgType.RealDmg) return dmg;
        return dmg - m_onHitBox.m_defence;
    }

}
