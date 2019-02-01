 using System.Collections.Generic;
 using System.Collections;
 using System;
 using UnityEngine;
 public enum SimonStatus {
     Walk,
     JumpUp0,
     JumpUp1,
     JumpFall0,
     JumpFall1,
     Idle,
     Squat,
     MeleeAttack,
     SquatMeleeAttack,
     OnHit0,
     OnHit1,
     OnHit2,
     Dead,

 }

 public enum ColliderType {
     Default = 0,
     Squat = 1,
     OnHit = 2,
     None = -1,
 }

 public class Simon : MovingUnit {

     public int m_jumpForece = 1000;
     private Animator m_anim;
     public SimonStatus m_status;
     private SimonStatus m_lastStatus;
     private Rigidbody2D m_rigidbody;
     public NonAlwaysActiveWeapons m_whip;
     private float m_lastJumpY;

     public int m_level = 0;

     public int m_maxLevel;

     [SerializeField]
     private NonAlwaysActiveWeapons[] m_whipEachLevel;

     [SerializeField]
     private Collider2D[] colliders;
     /**
         0 -> Stand Collider
         1 -> Squat Collider
      */

     private bool m_wontBeHurt = false;

     public float m_wontBeHurtTime;

     public float m_onHit1Time;

     public float m_hurtJumpForce;

     private Damage m_diedDamage;

     public override void Upgrade (int change) {
         if (change > 0) {
             m_level += change;
             if (m_level > m_maxLevel) {
                 m_level = m_maxLevel;
                 //TODO:show effect anim
             }
         } else if (change < 0) {
             m_level += change;
             if (m_level < 0) {
                 m_level = 0;
                 //TODO:show effect anim
             }
         }
         // Upgrade Whip
         m_whip = m_whipEachLevel[m_level];
         m_dmg = m_whip.m_dmg;
     }

     public override void EnableInput () {
         GameManager.input.x_axis += Walk;
         GameManager.input.x_axis_add_a += JumpHorizontal;
         GameManager.input.y_axis_down += Squat;
         GameManager.input.no_key += Idle;
         GameManager.input.b += MeleeAttack;
         GameManager.input.y_axis_down_add_b += SquatMeleeAttack;
         GameManager.input.axis_x_add_down += SquatFlip;
     }

     public override void DisableInput () {
         GameManager.input.x_axis -= Walk;
         GameManager.input.x_axis_add_a -= JumpHorizontal;
         GameManager.input.y_axis_down -= Squat;
         GameManager.input.no_key -= Idle;
         GameManager.input.b -= MeleeAttack;
         GameManager.input.y_axis_down_add_b -= SquatMeleeAttack;
         GameManager.input.axis_x_add_down -= SquatFlip;
     }

     void OnEnable () {

     }

     //Must Use Custom Mat /Custom/Sprite/Flicker Shader
     void ActiveFlicker (bool active) {
         GetComponent<SpriteRenderer> ().material.SetInt ("_Active", active?1 : 0);
     }

     void OnDisable () {
         DisableInput ();
     }

     void MeleeAttackHitBoxEnable () {
         m_whip.ActiveCollider (true);
     }

     IEnumerator AttackOver () {
         yield return new WaitForEndOfFrame ();
         yield return new WaitForSeconds (m_anim.GetCurrentAnimatorStateInfo (0).length);
         m_whip.ActiveCollider (false);
         if (!OnHit ())
             ChangeState (SimonStatus.Idle);
     }

     protected override bool CanBeDamaged () {
         return !m_wontBeHurt && !m_isDead;
     }

     IEnumerator SquatAttackOver () {
         yield return new WaitForEndOfFrame ();
         yield return new WaitForSeconds (m_anim.GetCurrentAnimatorStateInfo (0).length);
         m_whip.ActiveCollider (false);
         if (!OnHit ())
             ChangeState (SimonStatus.Squat);
     }

     bool OnHit () {
         return (m_status == SimonStatus.OnHit0 || m_status == SimonStatus.OnHit1 || m_status == SimonStatus.OnHit2);
     }

     void Awake () {
         m_rigidbody = GetComponent<Rigidbody2D> ();
         m_anim = GetComponent<Animator> ();
         ChangeState (SimonStatus.Idle);
         Flip ();
     }

     protected override void Die (Damage dmg) {
         m_isDead = true;
         m_diedDamage = dmg;
     }

     void ActiveCollider (ColliderType type) {
         for (var i = 0; i < colliders.Length; i++) {
             if (i == (int) type) {
                 colliders[i].enabled = true;
                 m_currentCollider = colliders[i];
             } else {
                 colliders[i].enabled = false;
             }
         }
     }

     void ChangeState (SimonStatus state) {
         if (m_status != state) {
             m_lastStatus = m_status;
             m_status = state;
         } else {
             return;
         }
         //  Debug.Log ("SwitchStatus:" + state);
         switch (state) {
             case SimonStatus.JumpUp0:
                 m_anim.Play ("Idle");
                 ActiveCollider (ColliderType.Default);
                 break;
             case SimonStatus.JumpUp1:
                 ActiveCollider (ColliderType.Squat);
                 m_anim.Play ("Squat");
                 break;
             case SimonStatus.Squat:
                 ActiveCollider (ColliderType.Squat);
                 if (m_lastStatus != SimonStatus.SquatMeleeAttack) {
                     transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - 7);
                 }
                 m_anim.Play ("Squat");
                 break;
             case SimonStatus.JumpFall0:
                 ActiveCollider (ColliderType.Squat);
                 break;
             case SimonStatus.JumpFall1:
                 m_anim.Play ("Idle");
                 ActiveCollider (ColliderType.Default);
                 break;
             case SimonStatus.Walk:
                 m_anim.Play ("Walk");
                 if (m_lastStatus == SimonStatus.Squat) {
                     transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 7);
                 }
                 ActiveCollider (ColliderType.Default);
                 break;
             case SimonStatus.Idle:
                 m_anim.Play ("Idle");
                 if (m_lastStatus == SimonStatus.Squat) {
                     transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 7);
                 }
                 ActiveCollider (ColliderType.Default);
                 break;
             case SimonStatus.MeleeAttack:
                 Debug.Log ("m_whip.m_animString:" + m_whip.m_animString);
                 m_anim.Play (m_whip.m_animString);
                 ActiveCollider (ColliderType.Default);
                 StartCoroutine (AttackOver ());
                 break;
             case SimonStatus.SquatMeleeAttack:
                 m_anim.Play (m_whip.m_animStringSquat);
                 ActiveCollider (ColliderType.Squat);
                 StartCoroutine (SquatAttackOver ());
                 break;
             case SimonStatus.OnHit0:
                 m_anim.Play ("Onhit");
                 ActiveCollider (ColliderType.OnHit);
                 DisableInput (); //No Input After Attack
                 OnDmgAction ();
                 new EnumTimer (() => {
                     ChangeState (SimonStatus.OnHit1);
                 }, 0.1f).StartTimeout (this);
                 break;

             case SimonStatus.OnHit1:
                 break;

             case SimonStatus.OnHit2:
                 m_anim.Play ("Squat");
                 transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - 7);
                 ActiveCollider (ColliderType.Squat);
                 m_rigidbody.velocity = new Vector2 (0, 0);
                 new EnumTimer (() => {
                     if (m_isDead) {
                         ChangeState (SimonStatus.Dead);
                     } else {
                         ChangeState (SimonStatus.Idle);
                         EnableInput ();
                     }
                 }, m_onHit1Time).StartTimeout (this);
                 break;
             case SimonStatus.Dead:
                 m_anim.Play ("Die");
                 transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - 8);
                 Destroy (m_rigidbody);
                 new EnumTimer (() => {
                     if (OnDied != null) {
                         OnDied (m_diedDamage);
                     }
                 }, 2f).StartTimeout (this);
                 break;

         }
     }

     void UpdateStatus () {
         switch (m_status) {
             case SimonStatus.Walk:
                 //Begin To Fall
                 if (m_rigidbody.velocity.y < 0) {
                     m_rigidbody.velocity = new Vector2 (0, m_rigidbody.velocity.y);
                     ChangeState (SimonStatus.JumpFall1);
                 }
                 break;
             case SimonStatus.JumpUp0:
                 var JumpHigh = GetJumpHeight ();
                 //  Debug.Log ("JumpHigh:" + JumpHigh);
                 if (JumpHigh > 5) {
                     ChangeState (SimonStatus.JumpUp1);
                 }
                 break;
             case SimonStatus.JumpUp1:
                 if (m_rigidbody.velocity.y < 0) {
                     ChangeState (SimonStatus.JumpFall0);
                 }
                 if (CheckGrounded ()) {
                     ChangeState (SimonStatus.Idle);
                 }
                 break;
             case SimonStatus.JumpFall0:
                 JumpHigh = GetJumpHeight ();
                 //  Debug.Log ("JumpHigh:" + JumpHigh);
                 if (JumpHigh < 8) {
                     ChangeState (SimonStatus.JumpFall1);
                 }
                 if (CheckGrounded ()) {
                     ChangeState (SimonStatus.Idle);
                 }
                 break;
             case SimonStatus.JumpFall1:
                 if (CheckGrounded ()) {
                     ChangeState (SimonStatus.Idle);
                 }
                 break;

             case SimonStatus.OnHit1:
                 if (CheckGrounded ()) {
                     ChangeState (SimonStatus.OnHit2); // 跪在地上
                 }
                 break;

         }
     }

     void RestrictPosition () {
         if (transform.position.x < InGameVars.LevelConfigs.m_levelMinX) {
             transform.position = new Vector2 (InGameVars.LevelConfigs.m_levelMinX, transform.position.y);
         }
         if (transform.position.x > InGameVars.LevelConfigs.m_levelMaxX) {
             transform.position = new Vector2 (InGameVars.LevelConfigs.m_levelMaxX, transform.position.y);
         }
     }

     protected override void OnDmg (Damage dmg) {
         var dmgXDirection = dmg.xDirection;
         if ((dmgXDirection.x < 0 && !facingRight) || (dmgXDirection.x > 0 && facingRight)) {
             Flip ();
         }
         ChangeState (SimonStatus.OnHit0);
     }

     void OnDmgAction () {
         m_lastJumpY = m_rigidbody.position.y;
         m_rigidbody.AddForce (new Vector2 (0, m_hurtJumpForce));
         m_rigidbody.velocity = new Vector2 ((facingRight ? -1 : 1) * m_moveSpeed, m_rigidbody.velocity.y);
         m_wontBeHurt = true;
         ActiveFlicker (true);
         new EnumTimer (() => { //无敌时间
             m_wontBeHurt = false;
             ActiveFlicker (false);
         }, m_wontBeHurtTime).StartTimeout (this);
     }

     void FixedUpdate () {
         UpdateStatus ();
         RestrictPosition ();
         CheckDropDie ();
     }

     void CheckDropDie () {
         if (m_isDead) return;
         if (CheckDropOutScreen ()) {
             m_isDead = true;
             new EnumTimer (() => { //无敌时间
                 if (OnDied != null) {
                     OnDied (m_diedDamage);
                 }
             }, 1.5f).StartTimeout (this);

         }
     }

     float GetJumpHeight () {
         return m_rigidbody.position.y - m_lastJumpY;
     }

     bool CheckIsOnGroundActions () {
         return CheckGrounded () && m_status == SimonStatus.Idle || (m_status == SimonStatus.Walk) || (m_status == SimonStatus.Squat);
     }

     bool CheckNotAttacking () {
         return m_status != SimonStatus.MeleeAttack && m_status != SimonStatus.SquatMeleeAttack;
     }

     public void Idle () {
         if (m_status == SimonStatus.Idle) return;
         if (!CheckNotAttacking ()) return;
         if (CheckIsOnGroundActions ()) {
             ChangeState (SimonStatus.Idle);
             m_rigidbody.velocity = new Vector2 (0, 0);
         }
     }

     public void SquatFlip (Vector2 direction) {
         if (m_status != SimonStatus.Squat) return;
         if (CheckNotAttacking ()) {
             if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight)) {
                 Flip ();
             }
         }
     }

     public void MeleeAttack () {
         if (CheckNotAttacking ()) {
             ChangeState (SimonStatus.MeleeAttack);
             if (CheckGrounded ()) {
                 m_rigidbody.velocity = new Vector2 (0, 0);
             }
         }
     }

     public void SquatMeleeAttack () {

         if (m_status != SimonStatus.Squat) return;
         if (CheckNotAttacking ()) {
             ChangeState (SimonStatus.SquatMeleeAttack);
             m_rigidbody.velocity = new Vector2 (0, 0);
         }
     }

     public void Walk (Vector2 direction) {
         if (m_status == SimonStatus.Squat) {
             ChangeState (SimonStatus.Walk);
             return;
         }
         if (CheckNotAttacking ())
             if (CheckIsOnGroundActions ()) {
                 m_rigidbody.velocity = new Vector2 (direction.x * m_moveSpeed, m_rigidbody.velocity.y);
                 if (m_rigidbody.velocity.x != 0) {
                     ChangeState (SimonStatus.Walk);
                 } else {
                     ChangeState (SimonStatus.Idle);
                 }
                 if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight)) {
                     Flip ();
                 }
             }
     }

     public void Squat () {
         if (m_status == SimonStatus.Squat) return;
         if (CheckNotAttacking ())
             if (CheckIsOnGroundActions ()) {
                 ChangeState (SimonStatus.Squat);
                 m_rigidbody.velocity = new Vector2 (0, 0);
             }
     }

     public void JumpHorizontal (int direction) {
         Walk (new Vector2 (direction, 0));
     }

     public void JumpHorizontal (Vector2 direction) {
         if (m_status == SimonStatus.Squat) return;
         if (CheckNotAttacking ())
             if (CheckIsOnGroundActions ()) {
                 m_lastJumpY = m_rigidbody.position.y;
                 m_rigidbody.AddForce (new Vector2 (0, m_jumpForece));
                 m_rigidbody.velocity = new Vector2 (direction.x * m_moveSpeed, m_rigidbody.velocity.y);
                 ChangeState (SimonStatus.JumpUp0);
                 if ((direction.x < 0 && facingRight) || (direction.x > 0 && !facingRight)) {
                     Flip ();
                 }
             }
     }

 }