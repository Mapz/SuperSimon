using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInit : IGameState {

    public override void OnUpdate () {
        if (GameManager.inited) {
            ChangeState<GameStateTitle> ();
        }
    }

}