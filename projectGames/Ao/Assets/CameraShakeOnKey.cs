using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeOnKey : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            impulseSource.GenerateImpulse();
        }
    }
}
