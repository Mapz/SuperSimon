using UnityEngine;
using UnityEngine.Events;

public class WanderBrick : BrickBase
{

    public UnityEvent[] m_OnDmgEvent;

    public Sprite m_SpriteWhenEmpty;

    protected override bool CanBeDamaged()
    {
        return m_HP >= 1;
    }

    protected override void OnDmg(Damage dmg)
    {
        m_OnDmgEvent[(int)m_HP].Invoke();
    }

    protected override void Die(Damage dmg)
    {
        if (null != GetComponent<Animator>())
        {
            Destroy(GetComponent<Animator>());
        }
        m_moveAfterHeadCollision = false;
        var ce = GetComponent<CustomEffector>();
        if (ce)
            Destroy(ce);
        GetComponent<SpriteRenderer>().sprite = m_SpriteWhenEmpty;
    }

    public void ProduceItem(GameObject ItemPrefab)
    {
        GameObject go = Instantiate(ItemPrefab);
        go.transform.position = transform.position;
        go.transform.SetParent(InGameVars.level.transform);
    }

    public void ProduceScore(GameObject ShowPrefab)
    {
        Debug.Log("ProduceCoin");
    }

    public void ProduceHeart(GameObject ShowPrefab)
    {
        GameObject go = Instantiate(ShowPrefab);
        go.transform.SetParent(this.transform);
        Destroy(go, go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        InGameVars.heart++;
    }

}