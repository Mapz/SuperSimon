using System;
using System.Collections.Generic;
public class StatusParams {
    private Dictionary<string, object> _content = new Dictionary<string, object> ();

    public StatusParams () {

    }
    public StatusParams (Dictionary<string, object> content) {
        _content = content;
    }

    public void Set (string key, object val) {
        _content[key] = val;
    }

    public T Get<T> (string key) where T : class {
        object ret;
        if (_content.TryGetValue (key, out ret)) {
            return (T) ret;
        }
        return null;
    }

    public object Get (string key) {
        object ret;
        if (_content.TryGetValue (key, out ret)) {
            return ret;
        }
        return null;
    }
}