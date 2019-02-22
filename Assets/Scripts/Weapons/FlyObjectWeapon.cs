using UnityEngine;
using System.Collections;

public class FlyObjectWeapon : Weapon
{
    [System.NonSerialized]
    public Unit m_Shooter;

    public bool m_canBeDestroyByShooter = false;

}
