using UnityEngine;

public class SubWeaponShooter : WeaponShooter
{

    private int m_level = 1; //Equals Max CombCount;
    private float m_CurrentCoolDown;
    [SerializeField]
    private float m_CoolDown;

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

    public override bool ExecShoot()
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

    protected override bool Shoot()
    {
        if (!base.Shoot()) return false;
        InGameVars.heart -= m_heartCost;
        return true;
    }

    private void FixedUpdate()
    {
        m_CurrentCoolDown -= Time.deltaTime;
    }
}
