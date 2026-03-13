using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

public class ScreenHealth : MonoBehaviour
{
    public Material screenDamageMat;
    [RangeAttribute(-1, 1)]
    public float IntensityVignette;

    public float TargetIntensity = 1f;
    [RangeAttribute(0, 1)]
    public float redValue;

    public float TargetRed = 1f;

    public int HitCounter = 4;

    public UnityEvent GameOver;

    void Start()
    {
        IntensityVignette = 1f;
    }

    void Update()
    {
        IntensityVignette = Mathf.Lerp(IntensityVignette, TargetIntensity, Time.deltaTime * 5f);
        screenDamageMat.SetFloat("_Vignette_radius", IntensityVignette);

        redValue = Mathf.Lerp(redValue, TargetRed, Time.deltaTime * 5f);
        screenDamageMat.SetColor("_Tint", new Color(redValue, 0f, 0f, 0f));
    }

    public void TakingDamage()
    {
        TargetIntensity = IntensityVignette - 0.25f;
        HitCounter--;

        if (HitCounter <= 0)
        {
            GameOver.Invoke();
            TargetIntensity = -1f;
        }
    }
}
