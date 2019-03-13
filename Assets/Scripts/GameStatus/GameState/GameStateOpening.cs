using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameStateOpening : IGameState
{

    PlayableDirector m_OpeningState;

    public override void OnEnter(StatusParams param = null)
    {
        m_OpeningState = Utility.LoadTimeline("OpenningStage");
        //if (loadUI)
        //{
        GameManager.StatusBar = Utility.CreateUI("UIGameState").GetComponent<StatusBar>();
        GameManager.StatusBar.transform.SetParent(GameManager.UICanvas.transform, false);
        //}
        new EnumTimer(() => { StartCoroutine(StartGame()); }, 5f).StartTimeout();
    }

    IEnumerator StartGame()
    {
        Object.Destroy(m_OpeningState.gameObject);
        yield return new WaitForSeconds(0.2f);
        StatusParams param = new StatusParams();
        param.Set("Init", true);
        param.Set("Level", "Level1");
        ChangeState<GameStateInGame>(param);
    }
}
