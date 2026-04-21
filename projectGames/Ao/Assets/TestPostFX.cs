using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class TestPostFX : MonoBehaviour
{
    public PostProcessVolume volume;
    public Slider slider;

    private Vignette vignette;
    private ChromaticAberration chroma;

    public CinemachineImpulseSource impulseSource;

    void Start()
    {
        // IMPORTANT: avoid editing shared profile
        volume.profile = Instantiate(volume.profile);

        // get vignette effect from profile
        volume.profile.TryGetSettings(out vignette);

        volume.profile.TryGetSettings(out chroma);
    }

    void Update()
    {
        // normalize slider (0–1)
        float percent = slider.value / slider.maxValue;

        // invert it (100 = no effect, 0 = full effect)
        float t = 1f - percent;

        // target vignette range: 0 → 0.66
        float target = Mathf.Lerp(0.4f, 0.66f, t);

        float targetChroma = t * 0.82f;

        // smooth transition
        vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value,target,Time.deltaTime * 1.5f);

        chroma.intensity.value = Mathf.MoveTowards(chroma.intensity.value,targetChroma,Time.deltaTime * 1.5f);


    }
}