using UnityEngine;
public enum DmgType {
    MeleeWhipPhysics,
    Item,
    HeadCollision,
}

public delegate int GetRealDmg (int rawDamage, DmgType dmgType);

public class Damage {
    private int _rawDamage;
    private DmgType _dmgType;
    private int _realDamage;
    private Vector2 _dmgDirection;
    private Vector2 _rawDirection;
    public DmgType dmgType {
        get { return _dmgType; }
    }

    public int rawDamage {
        get { return _rawDamage; }
    }

    public int realDamage {
        get {
            return _realDamage;
        }
    }

    public Vector2 rawDirection {
        get {
            return _rawDirection;
        }
    }

    public Vector2 dmgDirection { // 这会计算伤害方向，然后四舍五入成 四向 和 0 五种输出
        get {
            return _dmgDirection;
        }
    }

    public Vector2 xDirection {
        get {
            return new Vector2 (_rawDirection.x > 0 ? 1 : -1, 0);
        }
    }

    public Damage (int rawDamage, DmgType dmgType, Vector2 direction, GetRealDmg dmgDelegate) {
        _rawDamage = rawDamage;
        _dmgType = dmgType;
        _realDamage = dmgDelegate (_rawDamage, _dmgType);
        _rawDirection = direction;
        _dmgDirection = CalcDmgDirection (direction);
    }

    public Damage (int rawDamage, DmgType dmgType, GetRealDmg dmgDelegate) : this (rawDamage, dmgType, Vector2.zero, dmgDelegate) {

    }

    public Damage (int rawDamage, DmgType dmgType) : this (rawDamage, dmgType, (x, y) => { return rawDamage; }) {

    }

    public Damage (int rawDamage) : this (rawDamage, DmgType.MeleeWhipPhysics) {

    }

    private Vector2 CalcDmgDirection (Vector2 OriginalDirection) {
        var normalized = OriginalDirection.normalized;
        if (Mathf.Abs (OriginalDirection.x) > Mathf.Abs (OriginalDirection.y)) {
            return new Vector2 (OriginalDirection.x / Mathf.Abs (OriginalDirection.x), 0);
        } else if (Mathf.Abs (OriginalDirection.x) < Mathf.Abs (OriginalDirection.y)) {
            return new Vector2 (OriginalDirection.y / Mathf.Abs (OriginalDirection.y), 0);
        } else {
            return Vector2.zero;
        }
    }
}