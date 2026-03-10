using Unity.Cinemachine;
using UnityEngine;

public class hitBoxRegister : MonoBehaviour
{
    
    public AudioSource hit;
    public AudioClip hitSFX;
    public CinemachineImpulseSource impulseSource;

    void Start()
    {
        hit = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            hit.PlayOneShot(hitSFX);
            impulseSource.GenerateImpulse();
        }
    }
}
