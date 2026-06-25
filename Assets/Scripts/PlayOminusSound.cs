using FMODUnity;
using UnityEngine;

public class PlayOminusSound : MonoBehaviour
{

    public StudioEventEmitter ominousSoundEmmiter; //heb ik nu als naam gekozen om het voor mezelf overzichtelijk te houden, kan ook gwn een event zijn of emitter genoemd worden. makkelijk herbruikbaar
    private bool hasBeenPlayed = false;
    [SerializeField] private string triggerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag) && !hasBeenPlayed)    
        {
            ominousSoundEmmiter.Play();
            hasBeenPlayed = true;
        }
    }
}
