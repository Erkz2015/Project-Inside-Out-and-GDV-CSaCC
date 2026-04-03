using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCamera : MonoBehaviour
{
    public Vector2 rotationRange = new Vector3(70, 70);
    public float rotationSpeed = 10;
    public float dampingTime = 0.2f;
    public bool MouseInvis;

    Vector3 targetAngles;
    Vector3 followAngles;
    Vector3 followVelocity;
    Quaternion originalRotation;

    [Header("Hit Shake")]
    public float shakeDuration = 0.15f;
    public float shakeRotationStrength = 1.5f;

    float currentShakeTime;
    Vector3 shakeRot;

    void OnEnable()
    {
        MainMonster.OnMonsterAttack += TriggerShake;
    }

    void OnDisable()
    {
        MainMonster.OnMonsterAttack -= TriggerShake;
    }

    void Start()
    {
        originalRotation = transform.localRotation;
        MouseInvis = false;
        SwitchMouseVisibility();
    }

    void Update()
    {
        if (MouseInvis == false)
        {
            transform.localRotation = originalRotation;

            Vector2 mouseDelta = Vector2.zero;

            if (Mouse.current != null)
                mouseDelta = Mouse.current.delta.ReadValue();

            float inputH = mouseDelta.x;
            float inputV = mouseDelta.y;

            targetAngles.y += inputH * rotationSpeed * Time.deltaTime;
            targetAngles.x += inputV * rotationSpeed * Time.deltaTime;

            targetAngles.x = Mathf.Clamp(targetAngles.x, -rotationRange.x * 0.5f, rotationRange.x * 0.5f);

            followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, dampingTime);

            HandleShake();

            Quaternion baseRotation = originalRotation * Quaternion.Euler(-followAngles.x, followAngles.y, 0);
            Quaternion finalRotation = baseRotation * Quaternion.Euler(shakeRot);

            transform.localRotation = finalRotation;
        }
    }

    void HandleShake()
    {
        if (currentShakeTime > 0)
        {
            currentShakeTime -= Time.deltaTime;

            float t = currentShakeTime / shakeDuration;

            shakeRot = new Vector3(
                Random.Range(-1f, 1f) * shakeRotationStrength * t,
                Random.Range(-1f, 1f) * shakeRotationStrength * t,
                0f
            );
        }
        else
        {
            shakeRot = Vector3.Lerp(shakeRot, Vector3.zero, Time.deltaTime * 8f);
        }
    }

    public void TriggerShake()
    {
        currentShakeTime = shakeDuration;
    }

    public void SwitchMouseVisibility()
    {
        if (MouseInvis == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            MouseInvis = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MouseInvis = true;
        }
    }
}