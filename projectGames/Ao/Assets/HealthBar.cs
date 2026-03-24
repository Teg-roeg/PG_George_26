/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public Slider easeHealthBarSlider;

    public float maxFill = 100f;
    public float health;

    public float lerpSpeed = 27f;
    public float delayBeforeEase = 0.4f;

    private Coroutine easeCoroutine;

    void Start()
    {
        health = maxFill;

        healthBarSlider.maxValue = maxFill;
        easeHealthBarSlider.maxValue = maxFill;

        healthBarSlider.value = health;
        easeHealthBarSlider.value = health;
    }

    void Update()
    {
        if (healthBarSlider.value != health)
        {
            healthBarSlider.value = health;

            if (easeCoroutine != null)
                StopCoroutine(easeCoroutine);

            easeCoroutine = StartCoroutine(EaseHealthBar());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxFill);
    }

    IEnumerator EaseHealthBar()
    {
        yield return new WaitForSeconds(delayBeforeEase);

        while (easeHealthBarSlider.value != health)
        {
            easeHealthBarSlider.value = Mathf.MoveTowards(
                easeHealthBarSlider.value,
                health,
                lerpSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public Slider easeHealthBarSlider;

    public float lerpSpeed = 27f;
    public float delayBeforeEase = 0.4f;

    public MonoBehaviour healthSource; // drag object with Health script

    private IHealth health;
    private Coroutine easeCoroutine;

    void Start()
    {
        health = healthSource as IHealth;

        healthBarSlider.maxValue = health.MaxHealth;
        easeHealthBarSlider.maxValue = health.MaxHealth;

        healthBarSlider.value = health.CurrentHealth;
        easeHealthBarSlider.value = health.CurrentHealth;
    }

    void Update()
    {
        if (healthBarSlider.value != health.CurrentHealth)
        {
            healthBarSlider.value = health.CurrentHealth;

            if (easeCoroutine != null)
                StopCoroutine(easeCoroutine);

            easeCoroutine = StartCoroutine(EaseHealthBar());
        }
    }

    IEnumerator EaseHealthBar()
    {
        yield return new WaitForSeconds(delayBeforeEase);

        while (easeHealthBarSlider.value != health.CurrentHealth)
        {
            easeHealthBarSlider.value = Mathf.MoveTowards(
                easeHealthBarSlider.value,
                health.CurrentHealth,
                lerpSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}