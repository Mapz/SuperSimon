using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NormalBrick : BrickBase {

    public GameObject m_DestroyAnimPrefab;

    protected override bool CanBeDamaged () {
        return true;
    }

    protected override void Die (Damage dmg) {
        GameObject destroyAnim = Instantiate (m_DestroyAnimPrefab);
        destroyAnim.transform.position = transform.position;
        Destroy (gameObject);
    }

}