using UnityEngine;
public enum DmgType
{
    MeleeWhipPhysics,
    Item,
    HeadCollision,
    RealDmg,
}

public delegate float GetRealDmg(float rawDamage, DmgType dmgType);
public delegate void GetDmgDelegate(Damage dmg);

public class Damage
{
    public Unit Dealer
    {
        get; private set;
    }
    private readonly float _rawDamage;
    private DmgType _dmgType;
    private float _realDamage;
    private Vector2 _dmgDirection;
    private Vector2 _rawDirection;
    private Vector2 _dmgPosition;
    public DmgType dmgType
    {
        get { return _dmgType; }
    }

    public float rawDamage
    {
        get { return RawDamage; }
    }

    public float realDamage
    {
        get
        {
            return _realDamage;
        }
    }

    public Vector2 rawDirection
    {
        get
        {
            return _rawDirection;
        }
    }

    public Vector2 dmgDirection
    { // 这会计算伤害方向，然后四舍五入成 四向 和 0 五种输出
        get
        {
            return _dmgDirection;
        }
    }

    public Vector2 xDirection
    {
        get
        {
            return new Vector2(_rawDirection.x > 0 ? 1 : -1, 0);
        }
    }

    public float RawDamage => _rawDamage;

    public float RawDamage1 => _rawDamage;

    public DmgType DmgType { get => _dmgType; set => _dmgType = value; }
    public float RealDamage { get => _realDamage; set => _realDamage = value; }
    public Vector2 DmgDirection { get => _dmgDirection; set => _dmgDirection = value; }
    public Vector2 RawDirection { get => _rawDirection; set => _rawDirection = value; }
    public Vector2 DmgPosition { get => _dmgPosition; set => _dmgPosition = value; }

    public Damage(float rawDamage, DmgType dmgType, Vector2 direction, GetRealDmg dmgDelegate, Unit dealer, Vector2 position)
    {
        _rawDamage = rawDamage;
        _dmgType = dmgType;
        _realDamage = dmgDelegate(_rawDamage, _dmgType);
        _rawDirection = direction;
        _dmgDirection = CalcDmgDirection(direction);
        _dmgPosition = position;
        Dealer = dealer;
    }

    public Damage(float rawDamage, DmgType dmgType, GetRealDmg dmgDelegate, Unit dealer, Vector2 position) : this(rawDamage, dmgType, Vector2.zero, dmgDelegate, dealer, position)
    {

    }

    public Damage(float rawDamage, DmgType dmgType, Vector2 dmgdirection, GetRealDmg dmgDelegate, Unit dealer) : this(rawDamage, dmgType, dmgdirection, dmgDelegate, dealer, Vector2.zero)
    {

    }




    public Damage(float rawDamage, DmgType dmgType, GetRealDmg dmgDelegate, Unit dealer) : this(rawDamage, dmgType, Vector2.zero, dmgDelegate, dealer, Vector2.zero)
    {

    }

    public Damage(float rawDamage, DmgType dmgType, GetRealDmg dmgDelegate) : this(rawDamage, dmgType, Vector2.zero, dmgDelegate, null, Vector2.zero)
    {

    }

    public Damage(float rawDamage, DmgType dmgType) : this(rawDamage, dmgType, (x, y) => { return rawDamage; })
    {

    }

    public Damage(float rawDamage) : this(rawDamage, DmgType.MeleeWhipPhysics)
    {

    }

    private Vector2 CalcDmgDirection(Vector2 OriginalDirection)
    {
        var normalized = OriginalDirection.normalized;
        if (Mathf.Abs(OriginalDirection.x) > Mathf.Abs(OriginalDirection.y))
        {
            return new Vector2(OriginalDirection.x / Mathf.Abs(OriginalDirection.x), 0);
        }
        else if (Mathf.Abs(OriginalDirection.x) < Mathf.Abs(OriginalDirection.y))
        {
            return new Vector2(OriginalDirection.y / Mathf.Abs(OriginalDirection.y), 0);
        }
        else
        {
            return Vector2.zero;
        }
    }
}