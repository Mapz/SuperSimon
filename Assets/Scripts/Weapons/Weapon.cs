using UnityEngine;


public class Weapon : MonoBehaviour
{

    public Team m_team;
    public bool m_destroyEnviroment;
    public float m_dmg = 0;
    public Collider2D m_collider;
    public DmgType m_dmgType;
    public Unit m_WeaponCarrier;
    public bool m_DealDmgToCarrierWhenCollide;

    public virtual void OnDealDmg()
    {
        if (m_DealDmgToCarrierWhenCollide)
        {
            m_WeaponCarrier.GetDamage(new Damage(1));
        }

    }

}