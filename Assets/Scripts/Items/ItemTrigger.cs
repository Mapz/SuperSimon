using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemTrigger : MonoBehaviour
{
    public ItemType m_type;
    public int m_value;
    [System.NonSerialized]
    public Item m_baseObject;
    [System.NonSerialized]
    public GameObject m_SubWeaponPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (m_type)
            {
                case ItemType.heart:
                    InGameVars.heart += m_value;
                    break;
                case ItemType.life:
                    InGameVars.life += m_value;
                    break;
                case ItemType.score:
                    InGameVars.score += m_value;

                    break;
                case ItemType.upgrade:
                    InGameVars.hero.Upgrade(m_value);
                    break;
                case ItemType.hp:
                    InGameVars.hero.GetDamage(new Damage(m_value, DmgType.Item, (x, y) =>
                    {
                        return m_value;
                    }));
                    break;
                case ItemType.subWeapon:
                    ((Simon)InGameVars.hero).m_subWeaponShooter.SetShotObject(m_SubWeaponPrefab);
                    break;
                default:
                    break;
            }
            if (m_baseObject)
            {
                Destroy(m_baseObject.gameObject);
            }
            Destroy(gameObject);
        }
    }
}