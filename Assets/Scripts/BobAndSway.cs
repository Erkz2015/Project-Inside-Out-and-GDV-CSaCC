using UnityEngine;
using UnityEngine.InputSystem;

public class SwayNBobScript : MonoBehaviour
{
    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    public float smooth = 10f;
    float smoothRot = 12f;

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;

    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    [Header("Hit Shake")]
    public float shakeDuration = 0.2f;
    public float shakeStrength = 0.1f;
    public float shakeRotationStrength = 5f;

    float currentShakeTime;
    Vector3 shakePos;
    Vector3 shakeRot;

    Vector2 walkInput;
    Vector2 lookInput;


    void OnEnable()
    {
        MainMonster.OnMonsterAttack += TriggerShake;
    }

    void OnDisable()
    {
        MainMonster.OnMonsterAttack -= TriggerShake;
    }

    void Update()
    {
        GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        HandleShake();

        CompositePositionRotation();
    }

    void GetInput()
    {
        float x = 0f;
        float y = 0f;

        if (Keyboard.current.aKey.isPressed) x = -1f;
        if (Keyboard.current.dKey.isPressed) x = 1f;
        if (Keyboard.current.wKey.isPressed) y = 1f;
        if (Keyboard.current.sKey.isPressed) y = -1f;

        walkInput = new Vector2(x, y);

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        lookInput = mouseDelta;
    }

    void Sway()
    {
        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    void SwayRotation()
    {
        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    void BobOffset()
    {
        float moveAmount = walkInput.sqrMagnitude > 0 ? 1f : 0f;
        speedCurve += Time.deltaTime * (moveAmount * bobExaggeration);

        bobPosition.x = (curveCos * bobLimit.x) - (walkInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (walkInput.y * travelLimit.y);
        bobPosition.z = -(walkInput.y * travelLimit.z);
    }

    void BobRotation()
    {
        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
    }

    void HandleShake()
    {
        if (currentShakeTime > 0)
        {
            currentShakeTime -= Time.deltaTime;

            float shakeAmount = currentShakeTime / shakeDuration;

            shakePos = new Vector3(
                Random.Range(-1f, 1f) * shakeStrength * shakeAmount,
                Random.Range(-0.5f, 0.5f) * shakeStrength * shakeAmount,
                0
            );

            shakeRot = new Vector3(
                Random.Range(-1f, 1f) * shakeRotationStrength * shakeAmount,
                Random.Range(-1f, 1f) * shakeRotationStrength * shakeAmount,
                Random.Range(-1f, 1f) * shakeRotationStrength * shakeAmount
            );
        }
        else
        {
            shakePos = Vector3.Lerp(shakePos, Vector3.zero, Time.deltaTime * 10f);
            shakeRot = Vector3.Lerp(shakeRot, Vector3.zero, Time.deltaTime * 10f);
        }
    }

    public void TriggerShake()
    {
        float strengthMultiplier = 5f;

        currentShakeTime = shakeDuration;

        shakeStrength *= strengthMultiplier;
        shakeRotationStrength *= strengthMultiplier;
    }

    void CompositePositionRotation()
    {
        Vector3 finalPos = swayPos + bobPosition + shakePos;

        Quaternion finalRot =
            Quaternion.Euler(swayEulerRot) *
            Quaternion.Euler(bobEulerRotation) *
            Quaternion.Euler(shakeRot);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot, Time.deltaTime * smoothRot);
    }
}