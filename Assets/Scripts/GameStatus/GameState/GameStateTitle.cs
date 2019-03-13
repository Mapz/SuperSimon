using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateTitle : IGameState
{

    private GameObject UIGameState;
    private GameObject UITitle;
    private GameObject Level1;

    public override void OnEnter(StatusParams param = null)
    {
        // Load Title
        UIGameState = Utility.CreateUI("UIGameState");
        UIGameState.transform.SetParent(GameManager.UICanvas.transform, false);
        GameManager.StatusBar = UIGameState.GetComponent<StatusBar>();

        UITitle = Utility.CreateUI("UITitle");
        UITitle.transform.SetParent(GameManager.InGameUICanvas.transform, false);
        // Load Level1
        Level1 = Utility.LoadLevel("Level1");

        InGameVars.Init();

        GameManager.input.m_start = PressStart;

    }



    void PressStart()
    {
        GameManager.input.m_start -= PressStart;
        ChangeState<GameStateOpening>();
        StartCoroutine(GoToOpening());
    }

    IEnumerator GoToOpening()
    {
        Object.Destroy(UIGameState);
        Object.Destroy(UITitle);
        Object.Destroy(Level1);
        yield return new WaitForSeconds(0.2f);
        StatusParams param = new StatusParams();
        ChangeState<GameStateOpening>();
    }

}