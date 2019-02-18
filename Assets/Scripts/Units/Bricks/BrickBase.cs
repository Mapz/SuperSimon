using DG.Tweening;
using UnityEngine;
public abstract class BrickBase : Unit {
    public bool m_moveAfterHeadCollision;

    public virtual void OnHeadCollision (Damage dmg) {
        GetDamage (dmg);
    }

    protected override void OnAttack (Damage dmg) {
        if (m_moveAfterHeadCollision) {
            this.transform.DOLocalJump (Vector3.zero, 3, 0, 0.3f).SetRelative ();
        }
        base.OnAttack (dmg: dmg);
    }

    void OnCollisionEnter2D (Collision2D collision) {
        //TODO: PlayCollisionSound
        var colliderMovingUnit = collision.collider.GetComponent<MovingUnit> ();
        if (colliderMovingUnit && colliderMovingUnit.m_interactEnviroment) {
            if (colliderMovingUnit.m_TrigerBrick) {
                if (Utility.Collision.CheckCollisionDirection (GetComponent<BoxCollider2D> (), collision) == Utility.Collision.CollisionDirection.BOTTOM) {
                    if (m_moveAfterHeadCollision) {
                        this.transform.DOLocalJump (Vector3.zero, 3, 0, 0.3f).SetRelative ();
                    }
                    OnHeadCollision (new Damage (colliderMovingUnit.m_dmg, DmgType.HeadCollision, GetRealDmg));
                }
            }
        }

    }
}