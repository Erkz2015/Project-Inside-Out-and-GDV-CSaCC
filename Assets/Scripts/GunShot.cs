using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GunShot : MonoBehaviour
{
    public UnityEvent leftmouseActionHit;
    public UnityEvent leftmouseActionMis;
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
            leftmouseActionHit.Invoke();
        }
        else if (!monsterInside && Mouse.current.leftButton.wasPressedThisFrame)
        {
            leftmouseActionMis.Invoke();
        }
    }
}
