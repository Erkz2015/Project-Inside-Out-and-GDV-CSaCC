using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractZone : MonoBehaviour
{
    public UnityEvent leftmouseAction; 
    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    private void Update()
    {
        if (playerInside && Mouse.current.leftButton.wasPressedThisFrame)
        {
            leftmouseAction.Invoke();
        }
    }
}
