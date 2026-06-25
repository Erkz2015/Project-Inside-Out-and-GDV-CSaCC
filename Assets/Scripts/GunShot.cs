using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

public class GunShot : MonoBehaviour
{
    public static event Action LeftmouseActionHit;
    public static event Action LeftmouseActionMis;
    private bool monsterInside = false;
    [SerializeField] private string hitTag = "Monster";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hitTag))
        {
            monsterInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(hitTag))
        {
            monsterInside = false;
        }
    }

    private void Update()
    {
        if (monsterInside && Mouse.current.leftButton.wasPressedThisFrame)
        {
            LeftmouseActionHit?.Invoke();
        }
        else if (!monsterInside && Mouse.current.leftButton.wasPressedThisFrame)
        {
            LeftmouseActionMis?.Invoke();
        }
    }
}
