using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance;

    void Awake () {
        Instance = this;
    }

    void Start () {
        GameStatus.ChangeState<GameStateInit> ();
    }

}