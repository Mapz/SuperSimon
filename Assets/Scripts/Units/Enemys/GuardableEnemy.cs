using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GuardableEnemy : SimpleEnemy, IGuard
{

   

    [SerializeField]
    private int m_guardUpDefence;

    protected bool m_isGuarding;

    void Awake()
    {
        m_anim = GetComponent<Animator>();
    }

    public virtual void DisGuard()
    {
        m_anim.Play("Walk");
        m_defence -= m_guardUpDefence;
        m_isGuarding = false;
    }

    public virtual void Guard()
    {
        m_anim.Play("Guard");
        m_defence += m_guardUpDefence;
        m_isGuarding = true;
    }




}
