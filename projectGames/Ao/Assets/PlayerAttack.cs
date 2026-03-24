using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDuration = 0.8f;

    private Walking walking;
    private Animator animator;

    public AudioSource aud;

    private bool isAttacking;
    private bool canCombo;

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
            if (!isAttacking)
            {
                StartAttack();
            }
            else if (canCombo)
            {
                ComboAttack();
            }
        }
    }

    void StartAttack()
    {
        isAttacking = true;

        canCombo = true;

        walking.isAttacking = true;

        animator.SetTrigger("Attack");

        aud.Play();

        Invoke(nameof(StopAttack), attackDuration);
    }

    void ComboAttack()
    {
        canCombo = false;

        animator.SetTrigger("Attack2");

        aud.Play();

    }

    void StopAttack()
    {
        isAttacking = false;

        canCombo = false;
        
        walking.isAttacking = false;
    }
}