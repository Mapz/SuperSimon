using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonAlwaysActiveWeapons : Weapon {
    public string m_animString;
    public string m_animStringSquat;

    void awake () {
        gameObject.SetActive (false);

    }
    public void ActiveCollider (bool enabled) {
        gameObject.SetActive (enabled);
    }
}