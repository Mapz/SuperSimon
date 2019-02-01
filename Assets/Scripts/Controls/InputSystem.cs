using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void HorizontalMoveDelegate (Vector2 vec);
public delegate void KeyPressDelegate ();
public class InputSystem : MonoBehaviour {
    public HorizontalMoveDelegate x_axis;
    public HorizontalMoveDelegate x_axis_add_a;
    public KeyPressDelegate b;
    public KeyPressDelegate y_axis_down;
    public KeyPressDelegate y_axis_down_add_b;
    public KeyPressDelegate no_key;
    public KeyPressDelegate m_start;
    public HorizontalMoveDelegate axis_x_add_down;
    void Update () {

        //斜下攻击
        if (y_axis_down_add_b != null) {
            if (Input.GetKey ("s") && Input.GetKeyDown ("j")) {
                y_axis_down_add_b ();
                return;
            }
        }

        //斜下变向
        if (axis_x_add_down != null) {
            if (Input.GetKey ("s") && Input.GetKeyDown ("a")) {
                axis_x_add_down (new Vector2 (-1, 0));
                return;
            } else if (Input.GetKey ("s") && Input.GetKeyDown ("d")) {
                axis_x_add_down (new Vector2 (1, 0));
                return;
            }
        }

        //蹲下
        if (y_axis_down != null) {
            if (Input.GetKey ("s")) {
                y_axis_down ();
                return;
            }
        }

        //跳跃 
        if (x_axis_add_a != null) {
            if (Input.GetKey ("a") && Input.GetKeyDown ("k")) {
                x_axis_add_a (new Vector2 (-1, 0));
                return;
            } else if (Input.GetKey ("d") && Input.GetKeyDown ("k")) {
                x_axis_add_a (new Vector2 (1, 0));
                return;
            } else if (Input.GetKeyDown ("k")) {
                x_axis_add_a (new Vector2 (0, 0));
                return;
            }
        }

        //攻击
        if (b != null) {
            if (Input.GetKeyDown ("j")) {
                b ();
                return;
            }
        }

        //移动
        if (x_axis != null) {
            if (Input.GetKey ("a")) {
                x_axis (new Vector2 (-1, 0));
                return;
            } else if (Input.GetKey ("d")) {
                x_axis (new Vector2 (1, 0));
                return;
            }
        }

        //暂停
        if (m_start != null) {
            if (Input.GetKeyDown ("b")) {
                m_start ();
                return;
            }
        }

        if (no_key != null) {
            no_key ();
        }

    }

}