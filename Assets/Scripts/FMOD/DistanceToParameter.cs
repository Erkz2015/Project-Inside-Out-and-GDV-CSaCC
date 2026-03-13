using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class DistanceToParameter : MonoBehaviour
{
    public Transform target;
    public float minDistance = 1f;
    public float maxDistance = 20f;
    public string parameterName = "Range to Object";

    private StudioEventEmitter emitter;

    void Awake()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    void Update()
    {
        if (target == null || emitter == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);

        float parameterValue = Mathf.Lerp(0f, 1.95f, t);

        emitter.SetParameter(parameterName, parameterValue);
    }
}
