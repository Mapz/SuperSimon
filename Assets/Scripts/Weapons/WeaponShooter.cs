using UnityEngine;
using System.Collections;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField]
    protected GameObject toShootPrefab; //射擊出去的物件Prefab

    [SerializeField]
    protected MovingUnit m_Holder;

    protected virtual bool Shoot()
    {
        if (null == toShootPrefab) return false;

        GameObject shotObject = ObjectMgr<Unit>.Instance.Create(() =>
        {
            return Instantiate(toShootPrefab).GetComponent<Unit>();
        }).gameObject;

        shotObject.transform.SetParent(InGameVars.level.transform);
        shotObject.transform.position = transform.position;
        var flyingObject = shotObject.GetComponent<FlyingObject>();
        if (flyingObject.facingRight != m_Holder.facingRight)
        {
            flyingObject.Flip();
        }
        flyingObject.m_team = m_Holder.m_team;
        flyingObject.m_shooter = m_Holder;
        var weapon = flyingObject.GetComponentInChildren<FlyObjectWeapon>();
        if (null != weapon)
        {
            weapon.m_team = m_Holder.m_team;
            weapon.m_Shooter = m_Holder;
            weapon.m_WeaponCarrier = flyingObject;
        }
        return true;
    }

    public virtual bool ExecShoot()
    {
        return Shoot();
    }
}
