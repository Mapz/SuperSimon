using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour {
    private int _currCountdownValue;

    public int currCountdownValue {
        get { return _currCountdownValue; }
        set {
            if (_StatusBarAttached) {
                _StatusBarAttached.SetTime (value);
            }
            _currCountdownValue = value;
            
        }
    }

    private StatusBar _StatusBarAttached;
    private bool m_started = false;
    private IEnumerator StartCountdown (int countDownValue = 10) {
        if (m_started) yield break;
        currCountdownValue = countDownValue;
        m_started = true;
        while (currCountdownValue > 0) {
            yield return new WaitForSeconds (1.0f);
            currCountdownValue--;
        }
        m_started = false;
    }

    public void StartCountDown (int countDownValue) {
        StartCoroutine (StartCountdown (countDownValue));
    }

    public void AttachToStatusBar (StatusBar attach) {
        _StatusBarAttached = attach;
    }

    public void DisAttachToStatusBar () {
        _StatusBarAttached = null;
    }
}