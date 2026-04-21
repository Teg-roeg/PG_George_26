using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float comboResetTime = 1f; // time before combo resets

    private Walking walking;
    private Animator animator;
    public AudioSource aud;

    private int comboCounter = 0;
    private float lastClickTime;

    void Start()
    {
        walking = GetComponent<Walking>();
        animator = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleAttack();
        }

        // Reset combo if too slow
        if (Time.time - lastClickTime > comboResetTime)
        {
            comboCounter = 0;
        }
    }

    void HandleAttack()
    {
        if (Time.time - lastClickTime < 0.2f)
            return;

        lastClickTime = Time.time;

        comboCounter++;

        if (comboCounter == 1)
        {
            animator.SetTrigger("Attack");
        }
        else if (comboCounter == 2)
        {
            animator.SetTrigger("Attack2");
        }
        else if (comboCounter == 3)
        {
            animator.SetTrigger("Attack3");

            comboCounter = 0;
        }

        walking.isAttacking = true;
        aud.Play();

        Invoke(nameof(StopAttack), 0.75f);
    }

    void StopAttack()
    {
        walking.isAttacking = false;
    }
}