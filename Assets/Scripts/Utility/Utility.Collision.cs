//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Utility {
    /// <summary>
    /// 字符相关的实用函数。
    /// </summary>
    public static class Collision {
        public enum CollisionDirection {
            TOP,
            BOTTOM,
            LEFT,
            RIGHT,
            NONE,
        }

        public static CollisionDirection CheckCollisionDirection (BoxCollider2D colliderToCheck, Collision2D collision) {
            // Collider2D collider = collision.collider; //Subjective
            var RectWidth = colliderToCheck.size.x;
            var RectHeight = colliderToCheck.size.y;
            var circleRad = colliderToCheck.size.x;

            // Vector3 contactPoint = collision.contacts[collision.contacts.Length - 1].point;
            Vector3 center = colliderToCheck.bounds.center;
            foreach (var contact in collision.contacts) {
                var contactPoint = contact.point;
                if (contactPoint.y <= center.y &&
                    (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2)) {
                    return CollisionDirection.BOTTOM;
                } else if (contactPoint.y >= center.y && //checks that circle is on top of rectangle
                    (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2)) {
                    return CollisionDirection.TOP;
                } else if (contactPoint.x >= center.x &&
                    (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2)) {
                    return CollisionDirection.RIGHT;
                } else if (contactPoint.x <= center.x &&
                    (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2)) {
                    return CollisionDirection.LEFT;
                }
            }

            // GameObject a = new GameObject ();
            // a.transform.position = contactPoint;

            return CollisionDirection.NONE;

        }

        public static bool IsGrounded (MovingUnit unit, Transform m_groundCheck, LayerMask groundLayer, float verticalAnchor) {
            RaycastHit2D hit;
            var length = unit.GetComponent<SpriteRenderer> ().sprite.bounds.size.y * (1 - verticalAnchor) + 0.1f;
            Vector3 endPoint = Vector2.down * length;
            hit = Physics2D.Raycast (m_groundCheck.position, Vector2.down, length, 1 << LayerMask.NameToLayer ("Ground"));
            if (hit.collider != null) {
                Debug.DrawLine (m_groundCheck.position, hit.point, Color.red, 0.1f, false);
                return true;
            } else {
                Debug.DrawLine (m_groundCheck.position, m_groundCheck.position + endPoint, Color.green, 0.1f, false);
                return false;
            }
        }
    }

}