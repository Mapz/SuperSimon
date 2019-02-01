using System;
using System.Collections;
using UnityEngine;
public abstract class IGameState {
    public IGameState () {

    }
    public void ChangeState<T> (StatusParams param = null) where T : IGameState, new () {
        GameStatus.ChangeState<T> (param);
    }
    public virtual void OnEnter (StatusParams param = null) {
        // Debug.Log ("EnterState:" + this.ToString ());
    }
    public virtual void OnLeave () {
        // Debug.Log ("LeaveState:" + this.ToString ());
    }
    public virtual void OnUpdate () {
        // Debug.Log ("UpdateState:" + this.ToString ());
    }

    public void StartCoroutine (IEnumerator em) {
        GameStatus.StartCoroutineInstance (em);
    }
}