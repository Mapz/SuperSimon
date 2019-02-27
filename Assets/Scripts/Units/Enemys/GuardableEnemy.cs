using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GuardableEnemy : SimpleEnemy, IGuard
{

    private Animator m_anim;
    [SerializeField]
    private float m_wakeUpTime;
    [SerializeField]
    private int m_guardUpDefence;

    void Awake()
    {
        m_anim = GetComponent<Animator>();
    }

    public virtual void DisGuard()
    {
        m_anim.Play("Walk");
        m_defence -= m_guardUpDefence;
    }

    public virtual void Guard()
    {
        m_anim.Play("Guard");
        m_defence += m_guardUpDefence;
        StopX();
    }




}
