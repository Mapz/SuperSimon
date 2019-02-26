using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameStatus : MonoBehaviour
{
    public static GameStatus Instance;
    public static IGameState m_GameState;
    public static IGameState m_LastGameState;

    private static Dictionary<Type, IGameState> m_statusStore = new Dictionary<Type, IGameState>();

    void Awake()
    {
        Instance = this;
    }

    public static void ChangeState<T>(StatusParams param = null) where T : IGameState, new()
    {
        if (m_GameState != null)
        {
            if (m_GameState.GetType() == typeof(T))
            {
                return;
            }
            m_GameState.OnLeave();
            m_LastGameState = m_GameState;
        }
        IGameState tryGetState;
        if (m_statusStore.TryGetValue(typeof(T), out tryGetState))
        {
            m_GameState = tryGetState;
        }
        else
        {
            m_GameState = new T();
            m_statusStore[typeof(T)] = m_GameState;
        }
        m_GameState.OnEnter(param);

    }

    public static bool IsState<T>() where T : IGameState
    {
        return m_GameState is T;
    }

    public static void StartCoroutineInstance(IEnumerator em)
    {
        Instance.StartCoroutine(em);
    }

    void Update()
    {
        m_GameState.OnUpdate();
    }

}