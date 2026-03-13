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
            // reset to original rotation
            transform.localRotation = originalRotation;

            Vector2 mouseDelta = Vector2.zero;

            if (Mouse.current != null)
            {
                mouseDelta = Mouse.current.delta.ReadValue();
            }

            float inputH = mouseDelta.x;
            float inputV = mouseDelta.y;

            targetAngles.y += inputH * rotationSpeed * Time.deltaTime;
            targetAngles.x += inputV * rotationSpeed * Time.deltaTime;

            // clamp within range
            //targetAngles.y = Mathf.Clamp(targetAngles.y, -rotationRange.y * 0.5f, rotationRange.y * 0.5f);
            targetAngles.x = Mathf.Clamp(targetAngles.x, -rotationRange.x * 0.5f, rotationRange.x * 0.5f);

            // smooth movement
            followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, dampingTime);

            // apply rotation
            transform.localRotation = originalRotation * Quaternion.Euler(-followAngles.x, followAngles.y, 0);
        }
    }

    public void SwitchMouseVisibility()
    {
        if (MouseInvis == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            MouseInvis = false;
        } else { 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MouseInvis = true;
        }
    }
}