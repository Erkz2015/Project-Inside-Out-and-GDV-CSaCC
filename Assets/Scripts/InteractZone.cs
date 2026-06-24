using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractZone : MonoBehaviour
{
    public UnityEvent leftmouseAction; 
    private bool playerInside = false;
    [SerializeField] private string targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
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
