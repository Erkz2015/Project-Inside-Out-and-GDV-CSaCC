using FMODUnity;
using UnityEngine;

public class PlayOminusSound : MonoBehaviour
{

    public StudioEventEmitter OminousSoundEmmiter;
    private bool hasBeenPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenPlayed)
        {
            OminousSoundEmmiter.Play();
            hasBeenPlayed = true;
        }
    }
}
