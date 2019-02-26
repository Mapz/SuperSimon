using UnityEngine;

public class CheckPointGO : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Unit>().m_team == Team.Hero)
        {
            OnHeroEnter();
        }
    }

    private void OnHeroEnter()
    {
        CheckPointStatus.SetCheckPoint(transform.position);
        Destroy(this.gameObject);
    }
}
