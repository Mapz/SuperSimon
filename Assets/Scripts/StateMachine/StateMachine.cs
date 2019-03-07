using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void onStateAction(SMState state);
public delegate void onStateTransAction(SMState state, SMState nextState);
public delegate bool checkBool();
public class SMState
{
    public SMState(string m_stateName)
    {
        StateName = m_stateName;
        dataBase.SetData(PassDataName, new DataBase());
    }

    public string StateName { get; private set; }
    public onStateAction OnEnterState;
    public onStateAction OnExitState;
    public Dictionary<string, onStateTransAction> OnStateTransTo = new Dictionary<string, onStateTransAction>();
    public onStateAction OnUpdate;
    public checkBool TransPreCheck;
    private DataBase dataBase = new DataBase();
    private const string PassDataName = "PassData";
    public bool canTranlateToSelf = false;



    public void PassData<T>(string name, T data)
    {
        dataBase.GetData<DataBase>(PassDataName).SetData<T>(name, data);
    }

    public T GetPassData<T>(string name)
    {
        return dataBase.GetData<DataBase>(PassDataName).GetData<T>(name);
    }

}

public class StateMachine
{
    private Dictionary<string, SMState> m_statusStore = new Dictionary<string, SMState>();
    private SMState m_currentState;
    private SMState m_lastState;
    private bool m_stateEntered = false;
    private DataBase m_database = new DataBase();

    public void SetData<T>(string name, T data)
    {
        m_database.SetData<T>(name, data);
    }

    public T GetData<T>(string name)
    {
        return m_database.GetData<T>(name);
    }

    public bool IsState(string name)
    {
        SMState tempState;
        if (!m_statusStore.TryGetValue(name, out tempState))
        {
            throw new Exception("没有这个状态");
        }
        return name == m_currentState.StateName;
    }

    public void PassData<T>(string name, T data)
    {
        m_currentState.PassData<T>(name, data);
    }

    public T GetPassedData<T>(string name)
    {
        if (m_lastState == null)
        {
            Debug.LogError("不能在这个时候取数据");
            return default(T);
        }
        else
        {
            return m_lastState.GetPassData<T>(name);
        }

    }

    public bool AddState(SMState state)
    {
        SMState tempState;
        if (m_statusStore.TryGetValue(state.StateName, out tempState))
        {
            Debug.Log("已经有这个状态了");
            return false;
        }
        m_statusStore.Add(state.StateName, state);
        return true;
    }

    public bool TransState(string name)
    {

        SMState nextState;
        if (!m_statusStore.TryGetValue(name, out nextState))
        {
            Debug.Log("状态：" + name + " 不存在");
            return false;
        }

        bool preCheckResult = true;
        if (nextState.TransPreCheck != null)
        {
            preCheckResult = nextState.TransPreCheck.Invoke();
        }

        if (!preCheckResult)
        {
            Debug.Log("PreCheck not passed");
            return false;
        }

        if (m_currentState != null)
        {
            if (!m_currentState.canTranlateToSelf && m_currentState.StateName == name)
            {
                Debug.Log("Can not trans to self");
                return false;
            }
            m_currentState.OnExitState?.Invoke(m_currentState);
            onStateTransAction transAction;
            if (m_currentState.OnStateTransTo.TryGetValue(name, out transAction))
            {
                transAction.Invoke(m_currentState, nextState);
            }
        }
        m_lastState = m_currentState;
        m_currentState = nextState;
        m_stateEntered = false;
        return true;

    }

    public void OnUpdate()
    {
        if (null == m_currentState) return;
        if (!m_stateEntered)
        {
            m_currentState.OnEnterState?.Invoke(m_currentState);
            m_stateEntered = true;
        }
        m_currentState.OnUpdate?.Invoke(m_currentState);

    }
}
