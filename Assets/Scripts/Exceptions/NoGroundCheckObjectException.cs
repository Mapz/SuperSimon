using System;

public class NoGroundCheckObjectException : Exception {
    public override string ToString () {
        return "没有地面检查Object：" + Message;
    }

    public NoGroundCheckObjectException (string message) : base (message) {

    }
}