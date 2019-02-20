using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour {
    private const int c_blockWidth = 4;
    public int m_HP = 3;
    void Start () {
        SetHPBar ();
    }

    public void SetHPBar () {
        GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, m_HP * c_blockWidth - 1);
    }

    public void SetHPBar (int hp) {
        m_HP = hp;
        GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, m_HP * c_blockWidth - 1);
    }

}