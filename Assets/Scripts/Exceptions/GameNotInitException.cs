using System;

public class GameNotInitException : Exception {
    public override string ToString () {
        return "游戏初始化错误：" + Message;
    }

    public GameNotInitException (string message) : base (message) {

    }
}