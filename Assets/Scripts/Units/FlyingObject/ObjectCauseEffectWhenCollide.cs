using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCauseEffectWhenCollide : FlyingObject
{
    public GameObject m_effectPrefab;
    protected override void OnFixedUpdate()
    {
        if (physicsObject.collided || physicsObject.grounded)
        {
            ShowEffect();
        }
    }

    private void ShowEffect()
    {
        GameObject go = Instantiate(m_effectPrefab);
        go.transform.SetParent(InGameVars.level.transform);
        go.transform.position = transform.position;
        var weapon = go.GetComponent<Weapon>();
        if (weapon)
        {
            weapon.m_team = m_team;
        }
        Destroy(gameObject);
    }
}
