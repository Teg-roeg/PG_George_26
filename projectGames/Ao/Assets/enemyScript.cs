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

    State currentState = State.Idle;
    Transform target;
    internal void ThePlayerIs(Walking player)
    {
        target = player.transform;
        currentState = State.Alert;
    }

    void Start()
    {

    }

    
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:

                break;
            case State.Alert:
                transform.LookAt(target);
                transform.position += transform.forward * Time.deltaTime;
                break;
        }
    }
}
