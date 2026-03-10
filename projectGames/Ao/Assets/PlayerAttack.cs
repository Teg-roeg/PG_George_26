using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDuration = 0.8f;

    private Walking walking;
    private Animator animator;

    public AudioSource aud;

    private bool isAttacking;

    void Start()
    {
        walking = GetComponent<Walking>();
        animator = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartAttack();
            aud.Play();

        }
    }

    void StartAttack()
    {
        isAttacking = true;

        walking.isAttacking = true;

        animator.SetTrigger("Attack");

        Invoke(nameof(StopAttack), attackDuration);
    }

    void StopAttack()
    {
        isAttacking = false;
        walking.isAttacking = false;
    }
}