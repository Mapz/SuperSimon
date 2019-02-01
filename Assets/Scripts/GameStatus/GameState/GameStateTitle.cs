using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateTitle : IGameState {

    private GameObject UIGameState;
    private GameObject UITitle;
    private GameObject Level1;

    public override void OnEnter (StatusParams param = null) {
        // Load Title
        UIGameState = Utility.CreateUI ("UIGameState");
        UIGameState.transform.SetParent (GameManager.UICanvas.transform, false);
        GameManager.StatusBar = UIGameState.GetComponent<StatusBar> ();

        UITitle = Utility.CreateUI ("UITitle");
        UITitle.transform.SetParent (GameManager.InGameUICanvas.transform, false);
        // Load Level1
        Level1 = Utility.LoadLevel ("Level1");

        InGameVars.Init ();

        GameManager.input.m_start = PressStart;

    }

    IEnumerator StartGame () {
        GameObject.Destroy (UIGameState);
        GameObject.Destroy (UITitle);
        GameObject.Destroy (Level1);
        yield return new WaitForSeconds (0.2f);
        StatusParams param = new StatusParams ();
        param.Set ("Init", true);
        param.Set ("Level", "Level1");
        ChangeState<GameStateInGame> (param);
    }

    void PressStart () {
        GameManager.input.m_start -= PressStart;
        StartCoroutine (StartGame ());
    }

}