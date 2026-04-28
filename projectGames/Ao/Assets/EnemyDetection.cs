using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Material myMaterial;
    public Material myMaterial2;
    Collider target;
    private int isCollide = 0;
    enum State
    {
        Idle,
        Alert,
        Attack
    }

    State currentState = State.Idle;

    void Update()
    {
        print(isCollide);
        switch (currentState)
        {
            case State.Idle:
                myMaterial.color = new Color(0f, 0f, 0f, 0f);
                myMaterial2.SetFloat("_Alpha", 0f);
                break;
            case State.Alert:
                float fade = Mathf.PingPong(Time.time, 0.6f);
                myMaterial.color = new Color(1f, 0f, 0f, fade);
                myMaterial2.SetFloat("_Alpha", 1f);
                break;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentState = State.Alert;
            target = other;
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentState = State.Idle;
            target = null;
        }
    }
}