using System.Collections.Generic;
using UnityEngine;
public class OnHitCountDown {
    private List<Collider2D> hitboxList = new List<Collider2D> ();

    private float _coolDown = 0.2f;

    public void SetCoolDown (float coolDown) {
        _coolDown = coolDown;
    }

    public bool OnHit (Collider2D hitbox) {
        if (hitboxList.Contains (hitbox)) {
            return false;
        } else {
            hitboxList.Add (hitbox);
            new EnumTimer (() => {
                hitboxList.Remove (hitbox);
            }, _coolDown).StartTimeout ();
            return true;
        }
    }
}