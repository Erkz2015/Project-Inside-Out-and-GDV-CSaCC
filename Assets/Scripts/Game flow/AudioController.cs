using FMODUnity;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private StudioEventEmitter menuMusic;
    [SerializeField] private StudioEventEmitter happyMusic;
    [SerializeField] private StudioEventEmitter unnervingMusic;

    [Header("Effects")]
    [SerializeField] private StudioEventEmitter slowBreathing;
    [SerializeField] private StudioEventEmitter fastBreathing;
    [SerializeField] private StudioEventEmitter transitionSound;

    public void PlayMenu()
    {
        menuMusic.Play();
    }

    public void StopMenu()
    {
        menuMusic.Stop();
    }

    public void StartGameAudio()
    {
        happyMusic.Play();
        slowBreathing.Play();
    }

    public void StopStartAudio()
    {
        happyMusic.Stop();
        slowBreathing.Stop();
    }

    public void StartMonsterTransition()
    {
        transitionSound.Play();
        slowBreathing.Stop();
    }

    public void StartMonster()
    {
        unnervingMusic.Play();
        fastBreathing.Play();
    }

    public void StopMonster()
    {
        fastBreathing.Stop();
    }
}