using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUseseHolderHP : Weapon
{
    public Unit m_Holder;

    public override void OnDealDmg()
    {
        base.OnDealDmg();
        m_Holder.GetDamage(new Damage(1));
    }

}
