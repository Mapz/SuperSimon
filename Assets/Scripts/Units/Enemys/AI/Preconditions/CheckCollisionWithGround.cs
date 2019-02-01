using System.Collections;
using BT;
using UnityEngine;

public class CheckCollisionWithGround : BTPrecondition {

    private MovingUnit unit;
    public CheckCollisionWithGround () {

    }

    public override void Activate (Database database) {
        base.Activate (database);
        unit = database.GetComponent<MovingUnit> ();

    }

    public override bool Check () {

        RaycastHit2D hit;
        Vector3 endPoint = (unit.facingRight?1: -1) * Vector2.right * (unit.GetComponent<SpriteRenderer> ().size.x / 2 + 2);
        hit = Physics2D.Raycast (unit.transform.position, (unit.facingRight?1: -1) * Vector2.right, unit.GetComponent<SpriteRenderer> ().size.x / 2 + 2, 1 << LayerMask.NameToLayer ("Ground"));
        Debug.DrawLine (unit.transform.position, unit.transform.position + endPoint, Color.green, 0.1f, false);
        if (hit.collider != null) {
            Debug.DrawLine (unit.transform.position, hit.point, Color.red, 0.01f, false);
            return true;
        } else {

            return false;
        }
    }

}