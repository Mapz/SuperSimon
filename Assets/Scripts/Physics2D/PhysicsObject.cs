using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObject : MonoBehaviour, IPause
{

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    public Vector2 targetVelocity = Vector2.zero;
    public bool grounded { get; private set; }
    public bool collided { get; private set; }
    protected Vector2 groundNormal = Vector2.up;
    protected Rigidbody2D rb2d;
    public Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected List<Unit> hitUnitBufferList = new List<Unit>(16);
    public Action ComputeVelocity;
    public float distanceToGound { get; private set; }
    private bool m_pause = false;

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ResetContactLayer();
    }

    public void ResetContactLayer() {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        if (m_pause) return;
        targetVelocity = Vector2.zero;
        ComputeVelocity?.Invoke();
    }

    void FixedUpdate()
    {
        if (m_pause) return;
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;
        collided = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);

        distanceToGound = GetGroundYDistance();
    }

    private float GetGroundYDistance()
    {
        float distance = 10000;
        if (rb2d)
        {
            int count = rb2d.Cast(Vector2.down, contactFilter, hitBuffer);

            for (int i = 0; i < count; i++)
            {
                distance = hitBuffer[i].distance < distance ? hitBuffer[i].distance : distance;
            }
            //Debug.Log("distance:" + distance);
        }
        return distance;
    }



    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            hitUnitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {

                // 手动为Unit添加Collide事件
                var unit = hitBufferList[i].collider.GetComponent<Unit>();
                if (unit)
                {
                    if (!hitUnitBufferList.Contains(unit))
                    {
                        unit.OnCollideWithPhysicalObject(hitBufferList[i], rb2d.GetComponent<Collider2D>());
                        hitUnitBufferList.Add(unit);
                    }

                }
                // 手动为Unit添加Collide事件 Over

                Vector2 currentNormal = hitBufferList[i].normal;

                Debug.DrawLine(hitBufferList[i].point, hitBufferList[i].point + Vector2.one, Color.red, 0.1f, false);


                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;

                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                else
                {
                    collided = true;
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }


                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;



            }


        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

    public void Pause(bool pause)
    {
        m_pause = pause;
    }
}