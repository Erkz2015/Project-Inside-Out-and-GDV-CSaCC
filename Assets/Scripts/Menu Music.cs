using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMusic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StudioEventEmitter MusicEmitter;
    public string pitch = "Pitch";

    public float minParameter = 0f;
    public float maxParameter = 8f;
    public float duration = 0.5f;

    private float currentValue;
    private float startValue;
    private float targetValue;
    private float timer;

    void Start()
    {
        currentValue = minParameter;
        startValue = minParameter;
        targetValue = minParameter;
        MusicEmitter.SetParameter(pitch, currentValue);
    }

    void Update()
    {
        if (currentValue == targetValue)
            return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        currentValue = Mathf.Lerp(startValue, targetValue, t);
        MusicEmitter.SetParameter(pitch, currentValue);

        if (t >= 1f)
        {
            currentValue = targetValue;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        startValue = currentValue;
        targetValue = maxParameter;
        timer = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        startValue = currentValue;
        targetValue = minParameter;
        timer = 0f;
    }
}