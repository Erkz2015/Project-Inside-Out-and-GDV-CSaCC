using UnityEngine;
using UnityEngine.Events;

public class ScreenHealth : MonoBehaviour
{
    public Material screenDamageMat;
    [RangeAttribute(-1, 1)]
    public float intensityVignette;

    public float targetIntensity = 1f;
    [RangeAttribute(0, 1)]
    public float redValue;

    public float targetRed = 1f;

    public int hitCounter = 5;

    public UnityEvent GameOver;

    void Start()
    {
        intensityVignette = 1f;
    }

    void OnEnable()
    {
        MainMonster.OnMonsterAttack += TakingDamage;
    }

    void OnDisable()
    {
        MainMonster.OnMonsterAttack -= TakingDamage;
    }

    void Update()
    {
        intensityVignette = Mathf.Lerp(intensityVignette, targetIntensity, Time.deltaTime * 5f);
        screenDamageMat.SetFloat("_Vignette_radius", intensityVignette);

        redValue = Mathf.Lerp(redValue, targetRed, Time.deltaTime * 5f);
        screenDamageMat.SetColor("_Tint", new Color(redValue, 0f, 0f, 0f));
    }

    public void TakingDamage()
    {
        targetIntensity = intensityVignette - 0.20f;
        hitCounter--;

        if (hitCounter <= 0)
        {
            GameOver.Invoke();
            targetIntensity = -1f;
        }
    }
}
