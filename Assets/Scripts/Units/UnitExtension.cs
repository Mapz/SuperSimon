using System;

public delegate int DefenceCalcDelegate(int defence);
public static class UnitExtension
{
    public static void SetAllHitBoxRealDmgDelegate(this Unit m_unit, GetRealDmg calcDelegate)
    {
        if (m_unit.m_onHitBoxes == null) return;
        for (int i = 0; i < m_unit.m_onHitBoxes.Length; i++)
        {
            m_unit.m_onHitBoxes[i].m_dmgDelegate = calcDelegate;
        }
    }


    public static void SetAllHitBoxActive(this Unit m_unit, bool active)
    {
        if (m_unit.m_onHitBoxes == null) return;
        for (int i = 0; i < m_unit.m_onHitBoxes.Length; i++)
        {
            m_unit.m_onHitBoxes[i].SetActive(active);
        }
    }

    public static void SetAllHitBoxDefence(this Unit m_unit, DefenceCalcDelegate defenceFunc)
    {
        if (m_unit.m_onHitBoxes == null) return;
        for (int i = 0; i < m_unit.m_onHitBoxes.Length; i++)
        {
            m_unit.m_onHitBoxes[i].m_defence = defenceFunc(m_unit.m_onHitBoxes[i].m_defence);
        }
    }
}
