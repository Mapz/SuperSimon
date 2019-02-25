using System;
using System.Collections;
using UnityEngine;

public class EnumTimer
{
    private Action action;
    private float timeToWait;
    private int m_loop;
    private Func<bool> m_pauseWhile;
    private bool m_frameMode;

    private Coroutine m_coroutine;

    public EnumTimer(Action _action, float _timeToWait, int _loop = 1, Func<bool> pauseCondition = null)
    {
        action = _action;
        timeToWait = _timeToWait;
        m_loop = _loop;
        m_pauseWhile = pauseCondition;
    }

    public EnumTimer SetFrameMode(bool isFrameMode = true)
    {
        m_frameMode = isFrameMode;
        return this;
    }

    public void StartTimeout(MonoBehaviour mb)
    {
        m_coroutine = mb.StartCoroutine(TimtToDo());
    }

    public void StartTimeout()
    {
        m_coroutine = Game.Instance.StartCoroutine(TimtToDo());
    }

    IEnumerator TimtToDo()
    {
        for (int i = 0; i < m_loop; i++)
        {
            if (null != m_pauseWhile)
            {
                yield return new WaitWhile(m_pauseWhile);
            }
            if (m_frameMode)
            {
                for (int j = 0; i < timeToWait; j++)
                {
                    yield return new WaitForFixedUpdate();
                }

            }
            else
            {
                yield return new WaitForSeconds(timeToWait);
            }
            action();
        }
    }
}