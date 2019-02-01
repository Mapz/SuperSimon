using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class ItemFromBrick : Item {

    void Start () {
        transform.DOMoveY (8, 0.5f).SetRelative ().OnComplete (() => {
            GetComponent<Collider2D> ().enabled = true;
        });
    }

}