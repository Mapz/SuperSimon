using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public bool m_DebugOn = false;
    private Dictionary<string, Action> m_CheatActions = new Dictionary<string, Action>();
    private string m_currentString = "";

    void Start()
    {
        m_CheatActions.Add("POWERUP", () =>
        {
            if (InGameVars.hero != null && InGameVars.hero is Simon)
            {
                ((Simon)InGameVars.hero).m_whip.m_dmg = 30;
            }

        });
    }


    KeyCode FetchKey()
    {
        var e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }


    void Update()
    {
        if (!m_DebugOn) return;
        if (Input.anyKeyDown)
        {
            var update = false;
            string key = FetchKey().ToString();
            m_currentString += key;
            foreach (string cheatKey in m_CheatActions.Keys)
            {
                if (cheatKey.StartsWith(m_currentString))
                {
                    if (cheatKey == m_currentString)
                    {
                        Action toExec;
                        m_CheatActions.TryGetValue(m_currentString, out toExec);
                        toExec?.Invoke();
                        break;
                    }
                    else
                    {
                        update = true;
                        break;
                    }

                }
            }
            if (!update)
            {
                m_currentString = "";
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (m_currentString == "") return;
        Handles.color = Color.red;
        if (InGameVars.hero != null)
            Handles.Label(InGameVars.hero.transform.position + Vector3.up * 8, m_currentString);
        else
            Handles.Label(Vector3.zero, m_currentString);
    }


}
