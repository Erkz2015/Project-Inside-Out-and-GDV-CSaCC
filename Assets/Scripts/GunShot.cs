using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

public class GunShot : MonoBehaviour
{
    public static event Action leftmouseActionHit;
    public static event Action leftmouseActionMis;
    private bool monsterInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            monsterInside = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            monsterInside = false;
        }
    }

    private void Update()
    {
        if (monsterInside && Mouse.current.leftButton.wasPressedThisFrame)
        {
            leftmouseActionHit?.Invoke();
        }
        else if (!monsterInside && Mouse.current.leftButton.wasPressedThisFrame)
        {
            leftmouseActionMis?.Invoke();
        }
    }
}
