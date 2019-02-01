using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WanderBrick : BrickBase {

    public UnityEvent[] m_OnDmgEvent;

    public Sprite m_SpriteWhenEmpty;

    protected override bool CanBeDamaged () {
        return m_HP >= 1;
    }

    protected override void OnDmg (Damage dmg) {
        m_OnDmgEvent[m_HP].Invoke ();
    }

    protected override void Die (Damage dmg) {
        if (null != GetComponent<Animator> ()) {
            Destroy (GetComponent<Animator> ());
        }
        m_moveAfterHeadCollision = false;
        GetComponent<SpriteRenderer> ().sprite = m_SpriteWhenEmpty;
    }

    protected override int GetRealDmg (int dmg, DmgType dmgType) {
        if (dmg > 1) return 1;
        return dmg;
    }

    public void ProduceItem (GameObject ItemPrefab) {
        GameObject go = Instantiate (ItemPrefab);
        go.transform.position = transform.position;
        go.transform.SetParent (InGameVars.level.transform);
    }

    public void ProduceScore (GameObject ShowPrefab) {
        Debug.Log ("ProduceCoin");
    }

    public void ProduceHeart (GameObject ShowPrefab) {
        GameObject go = Instantiate (ShowPrefab);
        go.transform.SetParent (this.transform);
        Destroy (go, go.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).length);
        InGameVars.heart++;
    }

}