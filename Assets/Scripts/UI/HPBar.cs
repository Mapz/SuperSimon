using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    private const int c_blockWidth = 4;
    private const int c_maxBlock = 16;
    private Unit m_unitAttacked;

    void Start()
    {
        RefreshHPBar();
    }

    public void Attach(Unit unit)
    {
        m_unitAttacked = unit;
        RefreshHPBar();
    }

    public void DisAttach()
    {
        m_unitAttacked = null;
        RefreshHPBar();
    }

    public void RefreshHPBar()
    {
        int blocks = 0;
        if (m_unitAttacked)
        {
            blocks = Mathf.CeilToInt(c_maxBlock * m_unitAttacked.m_HP / m_unitAttacked.m_maxHp);
        }
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, blocks * c_blockWidth - 1);
    }



}