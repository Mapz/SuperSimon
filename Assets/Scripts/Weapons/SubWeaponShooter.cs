using UnityEngine;

public class SubWeaponShooter : MonoBehaviour
{
    private GameObject toShootPrefab; //射擊出去的物件Prefab
    private int m_level = 1; //Equals Max CombCount;
    private float m_CurrentCoolDown;
    [SerializeField]
    private float m_CoolDown;
    [SerializeField]
    private MovingUnit m_Holder;
    [SerializeField]
    private int m_MaxLevel;
    [SerializeField]
    private float m_ComboCoolDown;
    private int m_comboCount = 0;
    private int m_heartCost = 0;
    private bool m_ShowOnStatusBar = false;

    private void Awake()
    {
        if (m_Holder.m_team == Team.Hero)
        {
            m_ShowOnStatusBar = true;
        }
    }


    public bool CanShoot
    {
        get { return m_CurrentCoolDown <= 0 && (null != toShootPrefab) && (InGameVars.heart >= m_heartCost); }
    }

    public bool ExecShoot()
    {
        if (m_CurrentCoolDown > 0) return false;
        if (!Shoot()) return false;
        m_comboCount++;
        if (m_comboCount >= m_level)
        {
            m_CurrentCoolDown = m_CoolDown;
            m_comboCount = 0;
        }
        else
        {
            m_CurrentCoolDown = m_ComboCoolDown;
        }
        return true;
    }


    public void SetShotObject(GameObject go, Sprite icon)
    {
        toShootPrefab = go;//Must be a prefab
        GameObject temp = Instantiate(go);
        m_heartCost = temp.GetComponent<SubWeaponData>().heartCost;
        if (m_ShowOnStatusBar)
        {
            GameManager.StatusBar.SetSubWeapon(icon);
        }
        Destroy(temp);
    }

    public void Upgrade()
    {
        m_level++;
        if (m_level >= m_MaxLevel)
        {
            m_level = m_MaxLevel;
        }
    }

    private bool Shoot()
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
        InGameVars.heart -= m_heartCost;
        return true;
    }

    private void FixedUpdate()
    {
        m_CurrentCoolDown -= Time.deltaTime;
    }
}
