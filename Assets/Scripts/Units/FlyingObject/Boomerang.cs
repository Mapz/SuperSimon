using UnityEngine;
using System.Collections;
public class Boomerang : FlyingObject
{
    [SerializeField]
    [Range(1, 100)]
    private int m_FlyDistance;

    [SerializeField]
    [Range(0.5f, 100)]
    private float m_TimeToTargetDistance;
    private float m_Acceleration;
    private float m_CurrentVelocityX;

    private FlyObjectWeapon m_weapon;

    protected override void UpdateSpeedX()
    {
        if (m_moving != 0)
        {
            m_CurrentVelocityX += m_Acceleration * Time.deltaTime;

            physicsObject.targetVelocity.x = m_moving * m_CurrentVelocityX * (facingRight ? 1 : -1);
        }

    }

    protected override void OnEnabled()
    {
        m_weapon = GetComponentInChildren<FlyObjectWeapon>();
        m_Acceleration = CalcAcc();
        m_CurrentVelocityX = m_xMoveSpeed;
        base.OnEnabled();
        StartCoroutine(SetCanbeDestroyByShooter());
    }

    IEnumerator SetCanbeDestroyByShooter()
    {
        yield return new WaitForSeconds(m_TimeToTargetDistance);
        m_weapon.m_canBeDestroyByShooter = true;

    }

    //计算加速度
    private float CalcAcc()
    {
        return (m_FlyDistance - m_TimeToTargetDistance * m_xMoveSpeed) * 2 / (m_TimeToTargetDistance * m_TimeToTargetDistance);
    }

}
