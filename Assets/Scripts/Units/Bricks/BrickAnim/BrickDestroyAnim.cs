using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BrickDestroyAnim : MonoBehaviour {
    public Animator upLeft;
    public Animator upRight;
    public Animator downLeft;
    public Animator downRight;

    void Start () {
        upLeft.Play ("BrickDestroyUpLeft");
        upRight.Play ("BrickDestroyUpRight");
        downLeft.Play ("BrickDestroyDownLeft");
        downRight.Play ("BrickDestroyDownRight");
        Destroy (gameObject, upLeft.GetCurrentAnimatorStateInfo (0).length);

    }

}