using UnityEngine;
using System.Collections;

public class LineFlyingObject : FlyingObject
{
    protected override Vector2 SetPosition()
    {
        transform.position += new Vector3(m_moveSpeed * (facingRight ? 1 : -1), 0, 0) * Time.deltaTime;
        return transform.position;
    }
}
