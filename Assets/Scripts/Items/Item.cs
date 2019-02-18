using UnityEngine;

public enum ItemType
{
    heart,
    upgrade,
    score,
    life,
    subWeapon,
    hp,
}
public class Item : MonoBehaviour
{
    public ItemType m_type;
    public int m_value;
    public GameObject m_subWeaponPrefab;

    void Awake()
    {
        GameObject m_trigger = new GameObject("ItemTrigger");
        BoxCollider2D m_triggerCollider = m_trigger.AddComponent<BoxCollider2D>();
        ItemTrigger m_triggerItem = m_trigger.AddComponent<ItemTrigger>();
        m_triggerItem.m_baseObject = this;
        m_triggerItem.m_type = m_type;
        m_triggerItem.m_value = m_value;
        m_triggerItem.m_SubWeaponPrefab = m_subWeaponPrefab;
        Rigidbody2D m_triggerRigid = m_trigger.AddComponent<Rigidbody2D>();
        m_triggerRigid.bodyType = RigidbodyType2D.Kinematic;
        m_triggerCollider.size = GetComponent<BoxCollider2D>().size;
        m_triggerCollider.isTrigger = true;
        m_trigger.layer = LayerMask.NameToLayer("ItemTrigger");
        m_trigger.transform.SetParent(transform, false);
    }

}