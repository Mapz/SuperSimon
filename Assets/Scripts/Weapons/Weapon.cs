using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour {

    public Team m_team;
    public bool m_destroyEnviroment;
    public int m_dmg = 0;
    public Collider2D m_collider;

    public DmgType m_dmgType;

}