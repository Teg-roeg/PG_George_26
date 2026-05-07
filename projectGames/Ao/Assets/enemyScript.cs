using System;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    enum State
    {
        Idle,
        Alert,
        Attack
    }

    Rigidbody rb;

    State currentState = State.Idle;
    Transform target;

    internal void ThePlayerIs(Walking player)
    {
        target = player.transform;
        currentState = State.Alert;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Idle:
                break;

            case State.Alert:
                if (target != null)
                {
                    Vector3 direction = target.position - rb.position;
                    direction.y = 0f;
                    direction.Normalize();

                    rb.MovePosition(
                        rb.position + direction * 2f * Time.fixedDeltaTime
                    );
                }
                break;

            case State.Attack:
                break;
        }
    }
}